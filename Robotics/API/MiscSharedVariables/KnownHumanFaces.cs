﻿using System;
using System.Collections.Generic;
using System.Text;
using Robotics.API;
using Robotics.HAL.Sensors;
using Robotics.Utilities;

namespace Robotics.API.MiscSharedVariables
{
	/// <summary>
	/// Gets access to an HumanFaceProfile variable stored in the Blackboard
	/// </summary>
	public class KnownHumanFaces : SharedVariable<KnownHumanFace[]>
	{
		#region Variables

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of StringSharedVariable
		/// </summary>
		/// <param name="commandManager">The CommandManager object used to communicate with the Blackboard</param>
		/// <param name="variableName">The name of the variable in the Blackboard</param>
		/// <param name="initialize">Indicates if the shared variable will be automatically initialized if the commandManager is different from null</param>
		/// <remarks>If there is no variable with the provided name in the blackboard, a new variable with the asociated name is created</remarks>
		public KnownHumanFaces(CommandManager commandManager, string variableName, bool initialize)
			: this(commandManager, variableName, null, initialize) { }

		/// <summary>
		/// Initializes a new instance of StringSharedVariable
		/// </summary>
		/// <param name="commandManager">The CommandManager object used to communicate with the Blackboard</param>
		/// <param name="variableName">The name of the variable in the Blackboard</param>
		/// <param name="value">The value to store in the shared variable if it does not exist</param>
		/// <param name="initialize">Indicates if the shared variable will be automatically initialized if the commandManager is different from null</param>
		/// <remarks>If there is no variable with the provided name in the blackboard, a new variable with the asociated name is created</remarks>
		public KnownHumanFaces(CommandManager commandManager, string variableName, KnownHumanFace[] value, bool initialize)
			: base(commandManager, variableName, value, initialize) { }

		/// <summary>
		/// Initializes a new instance of StringSharedVariable
		/// </summary>
		/// <param name="variableName">The name of the variable in the Blackboard</param>
		/// <remarks>If there is no variable with the provided name in the blackboard, a new variable with the asociated name is created</remarks>
		public KnownHumanFaces(string variableName)
			: this(null, variableName, null, false) { }

		/// <summary>
		/// Initializes a new instance of StringSharedVariable
		/// </summary>
		/// <param name="variableName">The name of the variable in the Blackboard</param>
		/// <param name="value">The value to store in the shared variable if it does not exist</param>
		/// <remarks>If there is no variable with the provided name in the blackboard, a new variable with the asociated name is created</remarks>
		public KnownHumanFaces(string variableName, KnownHumanFace[] value)
			: this(null, variableName, value, false) { }

		#endregion

		#region Events

		#endregion

		#region Properties

		/// <summary>
		/// Returns false
		/// </summary>
		public override bool IsArray
		{
			get { return true; }
		}

		/// <summary>
		/// Returns -1
		/// </summary>
		public override int Length { get { return this.BufferedData != null ? this.BufferedData.Length : -1; } }

		/// <summary>
		/// Returns "string"
		/// </summary>
		public override string TypeName
		{
			get { return "KnownHumanFace"; }
		}

		#endregion

		#region Methodos

		/// <summary>
		/// Deserializes the provided object from a string
		/// </summary>
		/// <param name="serializedData">String containing the serialized object</param>
		/// <param name="value">When this method returns contains the value stored in serializedData the deserialization succeeded, or zero if the deserialization failed. The deserialization fails if the serializedData parameter is a null reference (Nothing in Visual Basic) or its content could not be parsed. This parameter is passed uninitialized</param>
		/// <returns>true if serializedData was deserialized successfully; otherwise, false</returns>
		protected override bool Deserialize(string serializedData, out KnownHumanFace[] value)
		{
			List<KnownHumanFace> faces;
			KnownHumanFace currentFace;
			int cc;

			if (String.IsNullOrEmpty(serializedData) || (String.Compare("null", serializedData, true) == 0))
			{
				value = null;
				return true;
			}

			faces = new List<KnownHumanFace>();
			value = null;
			cc = 0;

			while ((serializedData.Length - cc) >= 15)
			{
				if (!Deserialize(serializedData, ref cc, out currentFace))
					return false;
				faces.Add(currentFace);
				while ((cc < serializedData.Length) && (serializedData[cc] != '{'))
					++cc;
			}
			value = faces.ToArray();
			return true;
		}

		/// <summary>
		/// Deserializes the provided object from a string
		/// </summary>
		/// <param name="serializedData">String containing the serialized object</param>
		/// <param name="cc">Read header for the serializedData string</param>
		/// <param name="value">When this method returns contains the value stored in serializedData the deserialization succeeded, or zero if the deserialization failed. The deserialization fails if the serializedData parameter is a null reference (Nothing in Visual Basic) or its content could not be parsed. This parameter is passed uninitialized</param>
		/// <returns>true if serializedData was deserialized successfully; otherwise, false</returns>
		protected bool Deserialize(string serializedData, ref int cc, out KnownHumanFace value)
		{
			int start;
			int end;
			string name;
			int patterns;

			if (String.IsNullOrEmpty(serializedData) || (String.Compare("null", serializedData.Substring(cc, 4), true) == 0))
			{
				value = null;
				cc += 4;
				return true;
			}

			value = null;

			if (serializedData.Length < 15)
				return false;

			// 1. Read open brace '{'
			if (serializedData[cc++] != '{')
				return false;
			// 2. Read white space
			if (serializedData[cc++] != ' ')
				return false;

			// 3. Read name
			// 3.1. Read escaped double quotes
			if (!Scanner.ReadChar('\\', serializedData, ref cc) || !Scanner.ReadChar('"', serializedData, ref cc))
				return false;
			// 3.2. Read Name
			start = cc;
			while (cc < serializedData.Length)
			{
				// 3.3. Read escaped double quotes
				if (Scanner.ReadChar('\\', serializedData, ref cc) && Scanner.ReadChar('"', serializedData, ref cc))
					break;
			}

			// 3.4. Extract person name
			end = cc - 2;
			if ((end - start) < 1)
				return false;
			name = serializedData.Substring(start, end - start);
			
			// 4. Read white space
			if (!Scanner.ReadChar(' ', serializedData, ref cc))
				return false;

			// 5. Read 
			if (!Scanner.XtractInt32(serializedData, ref cc, out patterns) || (patterns < 1))
				return false;

			// 6. Read white space
			if (!Scanner.ReadChar(' ', serializedData, ref cc))
				return false;

			// 7. Read closing brace '}'
			if (!Scanner.ReadChar('}', serializedData, ref cc))
				return false;

			try
			{
				value = new KnownHumanFace(name, patterns);
			}
			catch { value = null; return false; }
			return true;
		}

		/// <summary>
		/// Sserializes the provided object to a string
		/// </summary>
		/// <param name="value">Object to be serialized</param>
		/// <param name="serializedData">When this method returns contains value serialized if the serialization succeeded, or zero if the serialization failed. The serialization fails if the serializedData parameter is a null reference (Nothing in Visual Basic) or outside the specification for the type. This parameter is passed uninitialized</param>
		/// <returns>true if value was serialized successfully; otherwise, false</returns>
		protected override bool Serialize(KnownHumanFace[] value, out string serializedData)
		{
			StringBuilder sb;

			if (value == null)
			{
				serializedData = "null";
				return true;
			}

			sb = new StringBuilder();
			for (int i = 0; i < value.Length; ++i)
			{
				Serialize(value[i], sb);
				if (i < (value.Length - 1))
					sb.Append(' ');
			}
			serializedData = sb.ToString();
			return true;
		}

		/// <summary>
		/// Sserializes the provided object into a StrinBuilder object
		/// </summary>
		/// <param name="value">Object to be serialized</param>
		/// <param name="sb">The StringBuilder object where the serialized data will be written</param>
		/// <returns>true if value was serialized successfully; otherwise, false</returns>
		protected bool Serialize(KnownHumanFace value, StringBuilder sb)
		{
			if (value == null)
			{
				sb.Append("null");
				return true;
			}

			sb = new StringBuilder();
			// 1. Write open brace '{'
			// 2. Write white space
			sb.Append("{ ");
			
			// 3. Write quoted name followed by one space
			sb.Append("\\\"");
			sb.Append(value.Name);
			sb.Append("\\\" ");

			// 4. Write Pan
			sb.Append(value.Patterns.ToString());

			// 5. Write space
			sb.Append(' ');

			// 6. Write white space followed by closing brace '}'
			sb.Append(" }");

			return true;
		}

		#endregion
	}
}
