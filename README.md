octave-sharp
============

Octave runtime for C#.

Proof of concept ,just a basic glue  there is still much work to be done.

Licence:LGPL 3.0

Requirements:
Windows
.NET4.0 Client profile
Octave VS2010 libs in PATH

How things work ,till now

Install Octave VS2010 version for Windows. Git clone. Setup Octave.Core include libs path to Octave directory. Compile an poof.
Add octave bin folder to your main app path and you are ready to go.

```c#
dynamic oi = Octave.Runtime.GetIntepreter();
```
Gets the dynamic object which is used to interop with Octave. Any method called against it will be called via Octave e.g.

```c#
double rnd= oi.rand()

var day =oi.gmtime(oi.time()).yday;

```
Controlling the number of return values.

```c#
 var matrix = new[,] {{1.2, 1.3, 1.4}, {3.4, 5.6, 7.8},{1.7,9.1,8.4}};

 var e = oi.eig(matrix,nargout:2);
 double[,] evect=e[0];
 double[,] eval=e[1];

```
Supported data types.

<table>
  <tr>
    <th>C#</th><th>Octave</th>
  </tr>
  <tr>
    <td>System.Double</td><td>double</td>
  </tr>
  <tr>
    <td>System.Float</td><td>float</td>
  </tr>
  <tr>
    <td>System.(U)Int64</td><td>(u)int64</td>
  </tr>
  <tr>
    <td>System.(U)Int32</td><td>(u)int32</td>
  </tr>
  <tr>
    <td>System.(U)Int16</td><td>(u)int16</td>
  </tr>
  <tr>
    <td>System.SByte</td><td>int8</td>
  </tr>
  <tr>
    <td>System.Byte</td><td>uint8</td>
  </tr>
  <tr>
    <td>System.Boolean</td><td>bool</td>
  </tr>
  <tr>
    <td>System.String</td><td>string</td>
  </tr>
  <tr>
    <td>T[,]</td><td>Matrix</td>
  </tr>
  <tr>
    <td>System.Object[,]</td><td>Cell</td>
  </tr>
  <tr>
    <td>System.Dynamic.ExpandoObject</td><td>Struct</td>
  </tr>
</table>



