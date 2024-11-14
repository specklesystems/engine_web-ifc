#include <string>
#include <algorithm>
#include <vector>
#include <stack>
#include <cstdint>
#include <memory>
#include "modelmanager/ModelManager.h"
#include "version.h"
#include <iostream>
#include <fstream>

using namespace webifc::manager;
using namespace webifc::parsing;
using namespace webifc::geometry;
using namespace webifc::schema;

// Forward declarations of classes
class Model;
class Api;
class Mesh;
class Geometry;
class Vertex;

#if defined(_MSC_VER)
    //  Microsoft 
    #define EXPORT __declspec(dllexport)
    #define IMPORT __declspec(dllimport)
#elif defined(__GNUC__)
    //  GCC
    #define EXPORT __attribute__((visibility("default")))
    #define IMPORT
#endif

// Exposed C functions 
extern "C"
{
    EXPORT Api* InitializeApi();
    EXPORT void FinalizeApi(Api* api);
    EXPORT char* GetVersion();
    EXPORT Model* LoadModel(Api* api, const char* fileName);
    EXPORT ::Geometry* GetGeometryFromId(Api* api, Model* model, uint32_t id);
    EXPORT int GetNumGeometries(Api* api, Model* model);
    EXPORT ::Geometry* GetGeometryFromIndex(Api* api, Model* model, int32_t index);
    EXPORT int GetNumMeshes(Api* api, ::Geometry* geom);
    EXPORT uint32_t GetMaxId(Api* api, Model* model);
    EXPORT char* GetLine(Api* api, Model* model, uint32_t id);
    EXPORT uint32_t GetGeometryId(Api* api, ::Geometry* geom);
    EXPORT uint32_t GetGeometryType(Api* api, ::Geometry* geom);
    EXPORT Mesh* GetMesh(Api* api, ::Geometry* geom, int index);
    EXPORT uint32_t GetMeshId(Api* api, ::Mesh* mesh);
    EXPORT double* GetTransform(Api* api, Mesh* mesh);
    EXPORT double* GetColor(Api* api, Mesh* mesh);
    EXPORT int GetNumVertices(Api* api, Mesh* mesh);
    EXPORT Vertex* GetVertices(Api* api, Mesh* mesh);
    EXPORT int GetNumIndices(Api* api, Mesh* mesh);
    EXPORT uint32_t* GetIndices(Api* api, Mesh* mesh);
}

// Vertex data structure as used by the web-IFC engine
struct Vertex 
{
    double Vx, Vy, Vz;
    double Nx, Ny, Nz;
};

// Color data
struct Color 
{
    double R, G, B, A;
    Color() : R(0), G(0), B(0), A(0) {}
    Color(double r, double g, double b, double a)
        : R(r), G(g), B(b), A(a) {}
};

struct Mesh 
{
    IfcGeometry* geometry;
    Color color;
    uint32_t id;
    std::array<double, 16> transform;
    Mesh(uint32_t id) 
        : geometry(nullptr), id(id), transform({}), color() 
    { }
};

struct Geometry 
{
    uint32_t id;
    uint32_t type;
    IfcFlatMesh* flatMesh;
    std::vector<Mesh*> meshes;    
    Geometry(uint32_t id, uint32_t type)
        : id(id), flatMesh(nullptr), type(type) 
    {}
};

struct Property 
{
    uint32_t id;
    uint32_t type;  
    Property(uint32_t id, uint32_t type)
        : id(id), type(type) 
    {}
};


// Model class, abstraction over the web-IFC engine concept of Model ID
struct Model
{
    uint32_t id;
    IfcLoader* loader;
    IfcGeometryProcessor* geometryProcessor;
    std::vector<::Geometry*> geometryList;
    std::unordered_map<uint32_t, ::Geometry*> geometries;

    Model(IfcSchemaManager* schemas, IfcLoader* loader, IfcGeometryProcessor* processor, uint32_t id)
        : loader(loader), geometryProcessor(processor), id(id)
    {
        for (auto type : schemas->GetIfcElementList())
        {
            // TODO: maybe some of these elments are desired. In fact, I think there may be explicit requests for IFCSPACE?
            if (type == IFCOPENINGELEMENT
                || type == IFCSPACE
                || type == IFCOPENINGSTANDARDCASE)
            {
                continue;
            }

            for (auto eId : loader->GetExpressIDsWithType(type))
            {
                auto flatMesh = geometryProcessor->GetFlatMesh(eId);
                auto g = new ::Geometry(eId, type);
                for (auto& placedGeom : flatMesh.geometries)
                {
                    auto mesh = ToMesh(placedGeom);
                    g->meshes.push_back(mesh);
                }
                geometries[eId] = g;
                geometryList.push_back(g);
            }
        }        
    }

    std::string GetLine(uint32_t expressID)
    {
        if (!loader->IsValidExpressID(expressID)) return "";
        uint32_t lineType = loader->GetLineType(expressID);
        if (lineType==0) return "";

        loader->MoveToArgumentOffset(expressID, 0);

        auto arguments = GetArgs();

        std::string retVal;
        retVal += "\"ID\": " + std::to_string(expressID) + ", ";
        retVal += "\"type\": " + std::to_string(lineType) + ", ";
        retVal += "\"arguments\": " + arguments;
        retVal += "}";

        return retVal;
    }

//copied from cpp test
std::string GetArgs(bool inObject=false, bool inList=false)
{
    std::string arguments;
    size_t size = 0;
    bool endOfLine = false;
    while (!loader->IsAtEnd() && !endOfLine)
    {
        webifc::parsing::IfcTokenType t = loader->GetTokenType();

        switch (t)
        {
            case webifc::parsing::IfcTokenType::LINE_END:
            {
                endOfLine = true;
                break;
            }
            case webifc::parsing::IfcTokenType::EMPTY:
            {
                arguments += " Empty ";
                break;
            }
            case webifc::parsing::IfcTokenType::SET_BEGIN:
            {
                arguments += GetArgs(false, true);
                break;
            }
            case webifc::parsing::IfcTokenType::SET_END:
            {
                endOfLine = true;
                break;
            }
            case webifc::parsing::IfcTokenType::LABEL:
            {
                // read label
                std::string obj; 
                obj = " type: LABEL ";
                loader->StepBack();
                auto s=loader->GetStringArgument();
                // read set open
                loader->GetTokenType();
                obj += " value " + GetArgs(loader,true) + " ";
                arguments += obj;
                break;
            }
            case webifc::parsing::IfcTokenType::STRING:
            case webifc::parsing::IfcTokenType::ENUM:
            case webifc::parsing::IfcTokenType::REAL:
            case webifc::parsing::IfcTokenType::INTEGER:
            case webifc::parsing::IfcTokenType::REF:
            {
                loader->StepBack();
                std::string obj;
                if (inObject) obj = ReadValue(t);
                else {
                    std::string obj; 
                    obj += " type REF ";
                    obj += ReadValue(t) + " ";
                }
                arguments += obj;
                break;
            }
            default:
                break;
        }
    }
    return arguments;
}

//copied from cpp test
std::string ReadValue(webifc::parsing::IfcTokenType t)
{
    switch (t)
    {
    case webifc::parsing::IfcTokenType::STRING:
    {
        return loader->GetDecodedStringArgument();
    }
    case webifc::parsing::IfcTokenType::ENUM:
    {
        std::string_view s = loader->GetStringArgument();
        return std::string(s);
    }
    case webifc::parsing::IfcTokenType::REAL:
    {
        std::string_view s = loader->GetDoubleArgumentAsString();
        return std::string(s);
    }
    case webifc::parsing::IfcTokenType::INTEGER:
    {
        long d = loader->GetIntArgument();
        return std::to_string(d);
    }
    case webifc::parsing::IfcTokenType::REF:
    {
        uint32_t ref = loader->GetRefArgument();
        return std::to_string(ref);
    }
    default:
        // use undefined to signal val parse issue
        return "";
    }
}


    ::Geometry* GetGeometry(uint32_t id)
    {
        auto it = geometries.find(id);
        if (it == geometries.end())
            return nullptr;
        return it->second;
    }

    Mesh* ToMesh(IfcPlacedGeometry& pg) 
    {
        auto r = new Mesh(pg.geometryExpressID);
        r->color = Color(pg.color.r, pg.color.g, pg.color.b, pg.color.a);
        r->geometry = &(geometryProcessor->GetGeometry(pg.geometryExpressID));
        r->transform = pg.flatTransformation;
        return r;
    }

    uint32_t GetMaxId() {
        return loader->GetMaxExpressId();
    }
};

struct Api 
{
    ModelManager* manager;
    IfcSchemaManager* schemaManager;
    LoaderSettings* settings;

    Api() 
    {
        schemaManager = new IfcSchemaManager();
        manager = new ModelManager(false);
        manager->SetLogLevel(6); // Turns off logging
        settings = new webifc::manager::LoaderSettings();
    }   

    Model* LoadModel(const char* fileName)
    {
        auto modelId = manager->CreateModel(*settings);
        auto loader = manager->GetIfcLoader(modelId);
        std::ifstream ifs;
        // NOTE: may fail if the file has unicode characters. This needs to be tested  
        ifs.open(fileName, std::ifstream::in);
        loader->LoadFile(ifs);
        return new ::Model(schemaManager, loader, manager->GetGeometryProcessor(modelId), modelId);
    }
};

//==

Api* InitializeApi() {
    return new Api();
}

void FinalizeApi(Api* api) {
    delete api->manager;
    delete api->schemaManager;
    delete api->settings;
    delete api;
}

char* GetVersion() {
    const char* v = WEB_IFC_VERSION_NUMBER.data();
    return strdup(v);
}

Model* LoadModel(Api* api, const char* fileName) {
    return api->LoadModel(fileName);
}

double* GetTransform(Api* api, Mesh* mesh) {
    return mesh->transform.data();
}

double* GetColor(Api* api, Mesh* mesh) {
    return &mesh->color.R;
}

::Geometry* GetGeometryFromId(Api* api, Model* model, uint32_t id) {
    return model->GetGeometry(id);
}

int GetNumGeometries(Api* api, Model* model) {
    return model->geometries.size();
}

::Geometry* GetGeometryFromIndex(Api* api, Model* model, int32_t index) {
    return model->geometryList[index];
}

uint32_t GetMaxId(Api* api, Model* model) {
    return model->GetMaxId();
}

char* GetLine(Api* api, Model* model, uint32_t id) {
    auto l = model->GetLine(id);
    return strdup(l.data());
}

uint32_t GetGeometryId(Api* api, ::Geometry* geom) {
    return geom->id;
}

uint32_t GetGeometryType(Api* api, ::Geometry* geom) {
    return geom->type;
}

int GetNumMeshes(Api* api, ::Geometry* geom) {
    return geom->meshes.size();
}

Mesh* GetMesh(Api* api, ::Geometry* geom, int index) {
    return geom->meshes[index];
}

uint32_t GetMeshId(Api* api, ::Mesh* mesh) {
    return mesh->id;
}

int GetNumVertices(Api* api, Mesh* mesh) {
    return mesh->geometry->vertexData.size() / 6;
}

Vertex* GetVertices(Api* api, Mesh* mesh) {
    return reinterpret_cast<Vertex*>(mesh->geometry->vertexData.data());
}

int GetNumIndices(Api* api, Mesh* mesh) {
    return mesh->geometry->indexData.size();
}

uint32_t* GetIndices(Api* api, Mesh* mesh) {
    return mesh->geometry->indexData.data();
}
