using System;
using System.Diagnostics;
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
                    throw new OctaveArgumentException("Type of nargout argument  must be Int32", "nargout");
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

           

            result = null;
           var status= OctaveCore.Octave.Feval(binder.Name, args1, nargout,ref result);

            if (status != OctaveCore.Octave.FevalStatus.OK)
            {
                 throw new OctaveRuntimeException(OctaveCore.Octave.GetLastError());
            }


            return true;

          
        }
    }
}