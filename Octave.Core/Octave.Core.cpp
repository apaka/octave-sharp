// This is the main DLL file.

#include "stdafx.h"

#include "Octave.Core.h"

#include <msclr\marshal_cppstd.h>

#include "octave_marshall.h"

bool OctaveCore::Octave::Main( array<System::String^>^ args )
{
	string_vector argv (args->Length);

	for (int i=0;i<args->Length;i++){
		String^ name=args[i];

		argv(i)=msclr::interop::marshal_as<std::string>(name);
	}
	return octave_main (args->Length, argv.c_str_vec(), true)==1;
}

OctaveCore::Octave::FevalStatus OctaveCore::Octave::Feval( String^ name,array<Object^>^ args,int nargout, Object^% resault )
{
	
	octave_value_list f_arg;
	try{
		 f_arg=object_to_octave_value_list(args);
	}
	catch(std::bad_typeid&){
       return FevalStatus::BadArgument;
	}

	octave_value_list f_ret;

	reset_error_handler();

	try{
		f_ret = feval (msclr::interop::marshal_as<std::string>(name), f_arg, nargout);
	}catch(octave_interrupt_exception&){
		
		return FevalStatus::Interupted;
	}catch (std::bad_alloc&){
       
		return FevalStatus::BadAlloc;
	}

	if(error_state){
		
		return FevalStatus::Error;
	}


	try{
		resault=octave_list_to_object(f_ret);}
	catch(std::bad_typeid&){
	return FevalStatus::BadReturn;
	}

	return FevalStatus::OK;
}

void OctaveCore::Octave::Exit()
{
//do_octave_atexit();
}

String^ OctaveCore::Octave::GetLastError()
{
	return msclr::interop::marshal_as<String^>(last_error_message());
}

