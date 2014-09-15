To create new examples/demos:
1. Copy a folder under Examples/ or Workitems/
2. Rename and modify
3. Run the example in Debug mode
4. Press F12 to generate a thumbnail, then add it into the Images/ folder as a resource

The examples are found by reflection, see the MainWindow.GetExamples method.
Note that the window class must be decorated by an `ExampleAttribute`.