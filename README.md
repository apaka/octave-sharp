octave-sharp
============

Octave runtime of C#4.0 using dynamics

Just basic glue finished there is still much work to be done ,not to be used , proof of concept


```c#
 var oi = Octave.Runtime.GetIntepreter();
 var rnd = oi.rand();
 
 oi.cd("C:\test");
 
 var res = oi.myfunc(new[,] { { 1 }, { 2 } }, nargout: 2);
 
```