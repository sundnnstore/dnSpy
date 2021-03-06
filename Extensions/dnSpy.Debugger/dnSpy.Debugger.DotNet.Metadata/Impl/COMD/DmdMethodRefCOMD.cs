﻿/*
    Copyright (C) 2014-2017 de4dot@gmail.com

    This file is part of dnSpy

    dnSpy is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    dnSpy is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with dnSpy.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;

namespace dnSpy.Debugger.DotNet.Metadata.Impl.COMD {
	sealed class DmdMethodRefCOMD : DmdMethodRef {
		readonly DmdComMetadataReader reader;
		readonly IList<DmdType> genericTypeArguments;
		readonly (IntPtr addr, uint size) signature;

		public DmdMethodRefCOMD(DmdComMetadataReader reader, (IntPtr addr, uint size) signature, IList<DmdType> genericTypeArguments, DmdType declaringTypeRef, string name, DmdMethodSignature rawMethodSignature, DmdMethodSignature methodSignature)
			: base(declaringTypeRef, name, rawMethodSignature, methodSignature) {
			this.reader = reader ?? throw new ArgumentNullException(nameof(reader));
			this.genericTypeArguments = genericTypeArguments;
			this.signature = signature;
		}

		T COMThread<T>(Func<T> action) => reader.Dispatcher.Invoke(action);

		internal override DmdMethodSignature GetMethodSignatureCore(IList<DmdType> genericMethodArguments) => COMThread(() => reader.ReadMethodSignature_COMThread(signature, genericTypeArguments, genericMethodArguments, isProperty: false));
	}
}
