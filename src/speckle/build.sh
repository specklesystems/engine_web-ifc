export CC=/usr/bin/gcc
export CXX=/usr/bin/g++
cmake ../cpp/CMakeLists.txt -B ../cpp/build
cmake --build ../cpp/build --config Release
cp ../cpp/build/libweb-ifc.so .