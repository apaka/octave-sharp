using System.Runtime.ConstrainedExecution;

namespace Octave
{
    public static class Runtime
    {
        private static readonly OctaveIntepreter _intepreter;

        private static readonly RuntimeFinalizer _finalizer;

        static Runtime()
        {
           if(OctaveCore.Octave.Main(new[]{
                            "octave-sharp",
                            "--no-line-editing",
                            "--no-history",
                            "--no-init-file",
                            "--silent"}))
           {
               throw new OctaveRuntimeException("Runtime initalization error");
           }

            _finalizer = new RuntimeFinalizer();

            _intepreter = new OctaveIntepreter();
        }

        public static dynamic GetIntepreter()
        {
            return _intepreter;
        }

        #region Nested type: RuntimeFinalizer

        private class RuntimeFinalizer : CriticalFinalizerObject
        {
            ~RuntimeFinalizer()
            {
                OctaveCore.Octave.Exit();
            }
        }

        #endregion
    }
}