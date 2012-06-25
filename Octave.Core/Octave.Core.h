// Octave.Core.h

#pragma once
#include <config.h>
#include <symtab.h>
#include <octave.h>
#include <parse.h>
#include <msclr\marshal_cppstd.h>
#include <iostream>
#include "Array.h"
using namespace System;

namespace OctaveCore {

	private  ref class Octave
	{
	public:


		enum class FevalStatus {
			OK=0,
			NotImpl=1,
			BadAlocc=2,
			Exp=3


		};

		static bool Main(array<System::String^>^ args);

		static FevalStatus Feval(String^ name,array<Object^>^ args,int nargout, Object^% resault);

		static void Exit();

		static String^ GetLastError();


	private:

		template<typename T,typename O>
		static O array_to_octave_matrix( array<T,2>^ matrix){

			O marray(dim_vector(matrix->GetLength(0),matrix->GetLength(1)));
			pin_ptr<T> ptr=&matrix[0,0];
			memcpy(marray.fortran_vec(),ptr,marray.byte_size());

			return marray;



		}
		static octave_value object_to_octave_value(Object^ obj){


			if(obj->GetType()==array<double,2>::typeid){

				return array_to_octave_matrix<double,Matrix>(static_cast<array<double,2>^>(obj));
			}else if(obj->GetType()==array<int,2>::typeid){

				return array_to_octave_matrix<int,int32NDArray>(static_cast<array<int,2>^>(obj));
			}
			else if(obj->GetType()==double::typeid){
				return octave_value(static_cast<double>(obj));
			}
			else if(obj->GetType()==String::typeid){

				return octave_value(msclr::interop::marshal_as<std::string>(static_cast<String^>(obj)));
			}
			else {

				throw gcnew NotImplementedException();
			}



		}




		static octave_value_list array_to_octave_value_list(array<Object^>^ args){


			octave_value_list f_args(args->Length);

			for (int i=0;i<args->Length;i++)
			{

				Console::WriteLine(args[i]->GetType());


				f_args(i)=object_to_octave_value(args[i]);


			}


		

		return f_args;

	}


	static Object^  octave_list_to_object(octave_value_list& args){

		array<Object^>^ f_ret= gcnew array<Object^>(args.length());



		for (int i=0;i<args.length();i++)
		{

			f_ret[i]=octave_value_to_object(args(i));



		}

		if(f_ret->Length==1){
			return f_ret[0];
		}
		else
		{
			return f_ret;
		}

	}








	template<typename T,typename O> 
	static array<T,2>^  octave_matrix_to_array( O& marray){


		array<T,2>^ matrix = gcnew array<T,2>(marray.dim1(),marray.dim2());

		pin_ptr<T> ptr=&matrix[0,0];
		memcpy(ptr,marray.fortran_vec(),marray.byte_size());

		return matrix;



	}

	static Object^ octave_value_to_object(const octave_value& value){

		if(value.is_matrix_type()){

			if(value.is_double_type()){

				return octave_matrix_to_array<double,Matrix>(value.matrix_value());

			}
			else if(value.is_int32_type()){

				return octave_matrix_to_array<int,int32NDArray>(value.int32_array_value());

			}else if(value.is_bool_type()){

				return octave_matrix_to_array<bool,boolMatrix>(value.bool_matrix_value());

			}
		}else if(value.is_cell()){

			auto cell=value.cell_value();

			array<Object^,2>^ ret=gcnew array<Object^,2>(cell.dim1(),cell.dim2());

			for(int i=0;i<cell.dim1();i++)
				for(int j=0;j<cell.dim2();j++){


					ret[i,j]=octave_value_to_object(cell(i,j));


				}
				return ret;

		}else if(value.is_scalar_type()){

			if(value.is_double_type()){

				return value.double_value();

			}
			else if(value.is_int32_type()){

				return static_cast<int>(value.int32_scalar_value());

			}else if(value.is_bool_type()){

				return value.bool_value();

			}


		}else if(value.is_string()){


			return msclr::interop::marshal_as<String^>(value.string_value());
		}/*else(value.is_map()){
		 auto m=value.scalar_map_value();

		 auto exo=gcnew ExpandoObject();





		 }*/


		else{


			throw gcnew NotImplementedException();
		}


	}


};
}
