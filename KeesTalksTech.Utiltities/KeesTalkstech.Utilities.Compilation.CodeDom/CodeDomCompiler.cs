﻿using KeesTalksTech.Utilities.Compilation;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Reflection;

namespace KeesTalkstech.Utilities.Compilation.CodeDom
{
	/// <summary>
	/// Compiler that uses the <see cref="CSharpCodeProvider"/> for compilation.
	/// </summary>
	/// <seealso cref="KeesTalksTech.Utilities.Compilation.ICompiler" />
	/// <seealso cref="System.IDisposable" />
	public class CodeDomCompiler : ICompiler, IDisposable
	{
		private readonly CSharpCodeProvider compiler = new CSharpCodeProvider();

		/// <summary>
		/// Compiles the specified code the sepcified assembly locations.
		/// </summary>
		/// <param name="assemblyLocations">The assembly locations.</param>
		/// <param name="code">The code.</param>
		/// <returns></returns>
		public Assembly Compile(string[] assemblyLocations, string code)
		{
			var parameters = new CompilerParameters();
			parameters.GenerateExecutable = false;
			parameters.GenerateInMemory = true;

			foreach (string assemblyLocation in assemblyLocations)
			{
				parameters.ReferencedAssemblies.Add(assemblyLocation);
			}

			var result = compiler.CompileAssemblyFromSource(parameters, code);

			if (result.Errors.Count > 0)
			{
				throw new CodeDomCompilerException("Assembly could not be created.", result);
			}

			try
			{
				return result.CompiledAssembly;
			}
			catch(Exception ex)
			{
				throw new CodeDomCompilerException("Assembly could not be created.", result, ex);
			}
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			compiler.Dispose();
		}
	}
}
