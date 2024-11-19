cmake -B ..\cpp\build -G "Visual Studio 17" -A x64 -DCMAKE_GENERATOR_TOOLSET=v143
cmake --build ..\cpp\build --config Release
cp ..\cpp\build\Release\web-ifc.dll .