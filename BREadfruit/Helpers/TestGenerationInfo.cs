using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BREadfruit.Helpers
{
	public class TestGenerationInfo
	{


		public string EntityName { get; set; }
		public string EntityType { get; set; }
		public string EntityForm { get; set; }

		public string EntityBlock { get; set; }

		public int NumberOfDefaultClauses { get; set; }
		public int NumberOfActions { get; set; }
		public int NumberOfRules { get; set; }
		public int NumberOfTriggers { get; set; }
		public int NumberOfConstraints { get; set; }


	}


	public static class TestGenParameters
	{
		//public static string TestGenLine1Regex = "^[\t\\s]*;[\t\\s]*TESTGEN[\t\\s]*:[\t\\s]*DEFAULTS[\t\\s]*=[\t\\s]*[0-9]+[\t\\s]*,[\t\\s]*RULES[\t\\s]*=[\t\\s]*[0-9]+[\t\\s]*,[\t\\s]*ACTIONS[\t\\s]*=[\t\\s]*[0-9]+[\t\\s]*,[\t\\s]*TRIGGERS[\t\\s]*=[\t\\s]*[0-9]+[\t\\s]*,[\t\\s]*CONSTRAINTS[\t\\s]*=[\t\\s]*[0-9]+[\t\\s]*$";
		public static string TestGenLine1Regex = "^[\t\\s]*;[\t\\s]*TESTGEN[\t\\s]*"; // :[\t\\s]*DEFAULTS[\t\\s]*=[\t\\s]*[0-9]+[\t\\s]*,[\t\\s]*RULES[\t\\s]*=[\t\\s]*[0-9]+[\t\\s]*,[\t\\s]*ACTIONS[\t\\s]*=[\t\\s]*[0-9]+[\t\\s]*,[\t\\s]*TRIGGERS[\t\\s]*=[\t\\s]*[0-9]+[\t\\s]*,[\t\\s]*CONSTRAINTS[\t\\s]*=[\t\\s]*[0-9]+[\t\\s]*$";
	}

}
