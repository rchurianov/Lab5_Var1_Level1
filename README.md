# Lab5_Var1_Level1
Practical assignment # 5, level 1, variant 1

This laboratory assignment extends the Lab4_Var1 code with serialization.

The main functionality of the console application is rather simple:

* An object of type `Student` is created. The object has some elements in `Exam` and `Credit` collections. The data for
those elements should be entered from the console.
* A full copy of the `Student` object is created and both the initial object and the copy are printed to the console.
* Then a file with a `Student` data is deserialized.
  - The user is asked to input a file name.
  - If the file exists the application loads the file into a new `Student` object
  and prints the result.
  - If the file does not exist the application creates the file and serializes the `Student` object, created in the first step.
* Some data is added from the console to the object #2.
