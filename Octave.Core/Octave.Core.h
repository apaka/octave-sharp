// Octave.Core.h

#pragma once
#include <config.h>
#include <symtab.h>
#include <octave.h>
#include <parse.h>
#include <msclr\marshal_cppstd.h>
#include <iostream>
#include "Array.h"
#include "error.h"
using namespace System;

namespace OctaveCore {

	private  ref class Octave
	{
	public:


		enum class FevalStatus {
			OK=0,
			NotImplemented=1,
			BadAlloc=2,
			Interupted=3,
			Error=4,
			BadArgument=5,
			BadReturn=6,


		};

		static bool Main(array<System::String^>^ args);

		static FevalStatus Feval(String^ name,array<Object^>^ args,int nargout,[System::Runtime::InteropServices::OutAttribute] Object^% resault);

		static void Exit();

		static String^ GetLastError();


	private:

		


};
}
