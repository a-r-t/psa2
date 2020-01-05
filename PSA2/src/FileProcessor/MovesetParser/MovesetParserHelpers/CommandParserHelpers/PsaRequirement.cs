using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetParser.MovesetParserHelpers.CommandParserHelpers
{
    public class PsaRequirement
    {
        public int RequirementId { get; set; }
        public int InverseFlag { get; set; }

        public PsaRequirement(int requirementId, int inverseFlag)
        {
            RequirementId = requirementId;
            InverseFlag = inverseFlag;
        }

        public override string ToString()
        {
            // once requirements config is created, this will be implemented with requirement's name
            return $"Requirement: {RequirementId}, Inverse Flag: {GetInverseFlagAsBoolean()}";
        }

        public bool GetInverseFlagAsBoolean()
        {
            return InverseFlag == 32768 ? true : false;
        }

        /*
           Original Code:

                an1 = ((alm[n + 1] >> 16) & 0xFFFF);
                an2 = (alm[n + 1] & 0xFFFF);
                // if an1 is 0, it's normal, if it's 32768 (8000 in hex), I guess it's a Not
                if (an1 == 32768 || an1 == 0)
                {
                    // adds a not flag to the requirement
                    if (an1 >= 1)
                    {
                        rd1 += "Not ";
                    }
                    // first requirements are 0 - 80 ... 128 is 80 in hex
                    if (an2 < 128)
                    {
                        rd1 += ReqEtxd[an2];
                    }
                    // the rest of the requirements are 270F - 2725 -- 9999 is 270F and 10021 is 2725
                    else if (an2 >= 9999 && an2 <= 10021)
                    {
                        rd1 += ReqEtxd[an2 - 9871];
                    }
                    // otherwise, requirement is unknown
                    else
                    {
                        rd1 += an2.ToString("X");
                    }
                }
                else
                {
                    rd1 = rd1 + "6x" + alm[n + 1].ToString("X");
                }
        */
    }
}
