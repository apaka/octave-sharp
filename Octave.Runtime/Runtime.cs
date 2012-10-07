using System.Runtime.ConstrainedExecution;

namespace Octave
{
    public static class Runtime
    {
        private static readonly OctaveIntepreter _intepreter;

        private static readonly RuntimeFinalizer _finalizer;

        static Runtime()
        {
           
            var args = new[]
                           {
                               "octave-sharp",
                               "--no-line-editing",
                               "--no-history",
                               "--no-init-file",
                               "--silent"
                           };

            if (!string.IsNullOrWhiteSpace(OctaveConfiguration.Default.RuntimeArguments))
            {
                args = OctaveConfiguration.Default.RuntimeArguments.Split('\0');
            }

            if (!OctaveCore.Octave.Main(args))
            {
                throw new OctaveRuntimeException("Runtime initialization error");
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