using BusinessLogic.BusinessRules;
using DataAccess.Interfaces;
using Entities.DTO;
using Moq;
using ServiceBus.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Test.CommonTest;
using Xunit;

namespace Test.BusinessRules
{
    public class DnaMutantTest
    {
        private readonly Mock<IServiceBusSend> serviceBus;
        Mock<IDnaMutantRepository> dataAccessDnaMutant;
        public DnaMutantTest()
        {
            dataAccessDnaMutant = new Mock<IDnaMutantRepository>();
            serviceBus = new Mock<IServiceBusSend>();
        }

        [Fact]
        public async void TestDNAValid()
        {
            List<Petition> petitions = TestFile.Deserialize<Petition>(TestConstant.dnaFile, TestConstant.nameData);
            List<bool> results = TestFile.Deserialize<bool>(TestConstant.dnaFile, TestConstant.nameResult);
            List<string> nombrePrueba = TestFile.Deserialize<string>(TestConstant.dnaFile, TestConstant.namePrueba);
            for (int i = 0; i < petitions.Count; i++)
            {
                serviceBus.Setup(s => s.SendToQueueAsync(It.IsAny<string>(), It.IsAny<Object>()));

                DnaMutant dnaMutant = new DnaMutant(dataAccessDnaMutant.Object,serviceBus.Object);

                var result = await dnaMutant.ValidDNA(petitions[i].dna);
                Debug.WriteLine("Prueba = " + nombrePrueba[i]);
                Assert.Equal(result, results[i]);
            }
        }

        [Fact]
        public void TestDNAInvalid()
        {
            List<Petition> petitions = TestFile.Deserialize<Petition>(TestConstant.dnaFileInvalid, TestConstant.nameData);
            List<string> nombrePrueba = TestFile.Deserialize<string>(TestConstant.dnaFileInvalid, TestConstant.namePrueba);
            for (int i = 0; i < petitions.Count; i++)
            {
                serviceBus.Setup(s => s.SendToQueueAsync(It.IsAny<string>(), It.IsAny<Object>()));

                DnaMutant dnaMutant = new DnaMutant(dataAccessDnaMutant.Object, serviceBus.Object);

                Debug.WriteLine("Prueba = " + nombrePrueba[i]);
                _ = Assert.ThrowsAsync<ArgumentException>(async () => await dnaMutant.ValidDNA(petitions[i].dna));
            }
        }

        [Fact]
        public async void TestGetStadistics()
        {
            List<long> countMutants = TestFile.Deserialize<long>(TestConstant.getStadisticsFile, TestConstant.nameDataMutant);
            List<long> countHumans = TestFile.Deserialize<long>(TestConstant.getStadisticsFile, TestConstant.nameDataHuman);

            List<ResponseStats> results = TestFile.Deserialize<ResponseStats>(TestConstant.getStadisticsFile, TestConstant.nameResult);

            List<string> nombrePrueba = TestFile.Deserialize<string>(TestConstant.getStadisticsFile, TestConstant.namePrueba);


            for (int i = 0; i < countMutants.Count; i++)
            {

                dataAccessDnaMutant.Setup(s => s.GetMutantCountAsync(true)).ReturnsAsync(countMutants[i]);
                dataAccessDnaMutant.Setup(s => s.GetMutantCountAsync(false)).ReturnsAsync(countHumans[i]);

                DnaMutant dnaMutant = new DnaMutant(dataAccessDnaMutant.Object, serviceBus.Object);

                var result = await dnaMutant.StatsAsync();
                Debug.WriteLine("Prueba = " + nombrePrueba[i]);
                Assert.Equal(result.Ratio, results[i].Ratio);
            }
        }

    }
}
