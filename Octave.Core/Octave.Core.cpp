// This is the main DLL file.

#include "stdafx.h"

#include "Octave.Core.h"

#include <msclr\marshal_cppstd.h>



bool OctaveCore::Octave::Main( array<System::String^>^ args )
{
	string_vector argv (args->Length);

	for (int i=0;i<args->Length;i++){
		String^ name=args[i];


		argv(i)=msclr::interop::marshal_as<std::string>(name);

	}
	return octave_main (args->Length, argv.c_str_vec(), true)==0;
}

OctaveCore::Octave::FevalStatus OctaveCore::Octave::Feval( String^ name,array<Object^>^ args,int nargout, Object^% resault )
{
	System::Console::WriteLine("[{0}]= {1}({2})",nargout,name,args->Length);

	auto f_arg=array_to_octave_value_list(args);




	auto f_ret = feval (msclr::interop::marshal_as<std::string>(name), f_arg, nargout);

	if(error_state){

		return FevalStatus::Exp;	
	}

	Console::WriteLine("Args returned:{0}",f_ret.length());

	resault=octave_list_to_object(f_ret);

	return FevalStatus::OK;
}

void OctaveCore::Octave::Exit()
{

}

String^ OctaveCore::Octave::GetLastError()
{
	return msclr::interop::marshal_as<String^>(last_error_message());
}
