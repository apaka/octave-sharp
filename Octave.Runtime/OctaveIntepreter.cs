using System;
using System.Dynamic;
using System.Reflection;

namespace Octave
{
    internal class OctaveIntepreter : DynamicObject
    {
        private PropertyInfo _bInfo;

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            int nargout = 1;
            var args1 = args;

            if (binder.CallInfo.ArgumentNames.Count != 0 &&
                binder.CallInfo.ArgumentNames[binder.CallInfo.ArgumentNames.Count - 1].Equals("nargout",
                                                                                              StringComparison.Ordinal))
            {
                try
                {
                    nargout = (int) args[args.Length - 1];
                }
                catch (InvalidCastException)
                {
                    throw new ArgumentException("Type of nargout argument  must be Int32", "nargout");
                }

                args1 = new object[args.Length - 1];
                Array.Copy(args, 0, args1, 0, args1.Length);
            }
            else
            {
                if (_bInfo == null)
                {
                    _bInfo = binder.GetType().GetInterface(
                        "Microsoft.CSharp.RuntimeBinder.ICSharpInvokeOrInvokeMemberBinder")
                        .GetProperty("ResultDiscarded");
                }


                if ((bool) _bInfo.GetValue(binder, null))
                {
                    nargout = 0;
                }
            }


            var status = OctaveCore.Octave.Feval(binder.Name, args1, nargout, out result);

            switch (status)
            {
                case OctaveCore.Octave.FevalStatus.OK:
                    return true;
                case OctaveCore.Octave.FevalStatus.Error:
                    throw new OctaveRuntimeException(OctaveCore.Octave.GetLastError());
                case OctaveCore.Octave.FevalStatus.BadArgument:
                    throw new ArgumentException("Unsupported argument type", "args");
                case OctaveCore.Octave.FevalStatus.BadReturn:
                    throw new ArgumentException("Unsupported return type", "ret");
                case OctaveCore.Octave.FevalStatus.BadAlloc:
                    throw new OutOfMemoryException("Octave failed to allocate the required memory");
                case OctaveCore.Octave.FevalStatus.Interupted:
                    throw new OctaveRuntimeException("Octave interpreter was interrupted");
                default:
                    return false;
            }
        }
    }
}