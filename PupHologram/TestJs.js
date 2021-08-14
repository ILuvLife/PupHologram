
var fs = new ActiveXObject("Scripting.FileSystemObject");
var a = fs.CreateTextFile("C:\\temp\\testfile.txt", true);
a.WriteLine("This is a js test.");
a.Close();


var ho = new ActiveXObject("PupHologram.PupTopper");
ho.GameChanged("testGameName", "testManuName", "TestDef");


