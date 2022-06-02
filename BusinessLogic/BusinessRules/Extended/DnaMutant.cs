
using BusinessLogic.Validation;
using Common.Constants;
using Entities.Entities;
using MongoDB.Bson;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace BusinessLogic.BusinessRules
{
    public partial class DnaMutant
    {
        private double GetRatio(long mutant, long human)
        {
            if (mutant == 0) { return 0; }
            else if (human == 0) { return 0; }
            else { return (double)mutant / human; }
        }

        private async Task<bool> ValidateTestDuplicate(DnaMutantEntity dnaMutant)
        {
            var result = await dataAccessDnaMutant.GetTestByDNAAsync(dnaMutant.Data);
            return result == null ? false : true;
        }

        private async Task RegistryTest(DnaMutantEntity dnaMutant)
        {
            await dataAccessDnaMutant.InsertAsync(dnaMutant);
        }

        private bool ValidMutant()
        {
            if (ValidationHorizontal()) { return true; }
            else if (ValidationVertical()) { return true; }
            else if (ValidationDiagonalAsc()) { return true; }
            else if (ValidationDiagonalDesc()) { return true; }
            else { return false; }
        }

        private void ValidData()
        {
            if (!localDna.ValidMinSize())
            {
                throw new ArgumentException(Constants.ParameterInvalid, JsonSerializer.Serialize(localDna).ToString());
            }

            if (!localDna.ValidSize())
            {
                throw new ArgumentException(Constants.ParameterInvalid, JsonSerializer.Serialize(localDna).ToString());
            }

            if (!localDna.ValidData())
            {
                throw new ArgumentException(Constants.ParameterInvalid, JsonSerializer.Serialize(localDna).ToString());
            }
        }

        private bool ValidationHorizontal()
        {
            for (int i = 0; i < localDna.Count; i++)
            {
                if (ValidStringMutant(localDna[i])) { return true; }
            }
            return false;
        }

        private bool ValidationVertical()
        {
            for (int i = 0; i < localDna.Count; i++)
            {
                string stringDNA = "";
                for (int j = 0; j < localDna.Count; j++)
                {
                    stringDNA += localDna[j][i];
                }
                if (ValidStringMutant(stringDNA)) { return true; }
            }
            return false;
        }

        private bool ValidationDiagonalAsc()
        {
            int diagonalExist = ((localDna.Count() - Constants.MountSequence) * 2) + 1;
            int max = Constants.MountSequence;
            int min = 0;
            for (int i = 0; i < diagonalExist; i++)
            {
                string stringDNA = GetStringDiagonalAsc(min, max);
                if (ValidStringMutant(stringDNA)) { return true; }

                min = max == localDna.Count() ? min + 1 : 0;
                max = max != localDna.Count() ? max + 1 : max;
            }
            return false;
        }

        private string GetStringDiagonalAsc(int min, int max)
        {
            int aux = max - 1;
            string stringDNA = "";
            for (int i = min; i < max; i++)
            {
                stringDNA += localDna[aux][i];
                aux -= 1;
            }

            return stringDNA;
        }

        private bool ValidationDiagonalDesc()
        {

            for (int i = 0; i <= localDna.Count() - Constants.MountSequence; i++)
            {
                var stringDNA = GetStringDiagonalDesc(i, localDna.Count());

                if (ValidStringMutant(stringDNA.Item1)) { return true; }

                if (ValidStringMutant(stringDNA.Item2)) { return true; }
            }
            return false;
        }

        private Tuple<string, string> GetStringDiagonalDesc(int Min, int Max)
        {
            int aux = 0;
            string stringDNA1 = "";
            string stringDNA2 = "";
            for (int i = Min; i < Max; i++)
            {
                stringDNA1 += localDna[aux][i];
                stringDNA2 += localDna[i][aux];
                aux += 1;
            }
            return Tuple.Create(stringDNA1, Min != 0 ? stringDNA2 : "");
        }

        private bool ValidStringMutant(string value)
        {
            char character = 'n';
            int amount = 0;
            foreach (var item in value)
            {
                if (character == item)
                {
                    amount += 1;
                    if (amount == Constants.MountSequence - 1)
                    {
                        amountString += 1;
                        amount = 0;
                        character = 'n';
                        if (amountString > 1) { return true; }
                    }
                }
                else
                {
                    amount = 0;
                    character = item;
                }
            }
            return false;
        }

        private async Task<bool> SendMessaggeServiceBus(bool isMutant)
        {

            DnaMutantEntity dnaMutant = new DnaMutantEntity
            {
                Id = ObjectId.GenerateNewId(),
                IsMutant = isMutant,
                Data = JsonSerializer.Serialize(localDna).ToString()
            };

            return await serviceBusSend.SendToQueueAsync(Constants.QueueName, dnaMutant);
        }
    }
}
