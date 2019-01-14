To compile clingo to JavaScript that is suitable for this App, change in clingo/app/web the file CMakeLists.txt:
- Add options
     -s WASM=0 --memory-init-file 0
  to
    set_target_properties(web PROPERTIES LINK_FLAGS
        "-s EXPORTED_FUNCTIONS='[\"_run\"]' -s EXTRA_EXPORTED_RUNTIME_METHODS='[\"ccall\"]'")
  to make sure that everything is compiled into a single .js file