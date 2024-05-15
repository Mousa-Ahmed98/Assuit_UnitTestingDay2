using CarAPI.Entities;
using CarAPI.Models;
using CarAPI.Payment;
using CarAPI.Repositories_DAL;
using CarAPI.Services_BLL;
using CarFactoryAPI.Entities;
using CarFactoryAPI.Repositories_DAL;
using CarFactoryAPI_test.stups;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace CarFactoryAPI_test
{
    public class OwnerServiceTests : IDisposable
    {
        private readonly ITestOutputHelper outputHelper;
        // Create Mock of the dependencies
        Mock<ICarsRepository> carRepoMock;
        Mock<IOwnersRepository> ownersRepoMock;
        Mock<ICashService> cashServiceMock;

        // use the fake object as dependency
        OwnersService ownersService;

        public OwnerServiceTests(ITestOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;
            // Test Start up
            outputHelper.WriteLine("Test start up");

            // Create Mock of the dependencies
            carRepoMock = new();
            ownersRepoMock = new();
            cashServiceMock = new();

            // use the fake object as dependency
            ownersService = new OwnersService(carRepoMock.Object,
                ownersRepoMock.Object, cashServiceMock.Object);

        }

        public void Dispose()
        {
            outputHelper.WriteLine("Test clean up");
        }
        [Fact]
        [Trait("Author", "Ahmed")]

        public void BuyCar_CarNotExist_NotExist() 
        {
            outputHelper.WriteLine("Test 1");
            // Arrange
            FactoryContext factoryContext = new FactoryContext();

            // CarRepository carRepository = new CarRepository(factoryContext);

            // Fake Dependency
            CarRepoStup carRepoStup = new CarRepoStup();

            // Real Dependency
            OwnerRepository ownerRepository = new OwnerRepository(factoryContext);
            CashService cashService = new CashService();

            OwnersService ownersService = new OwnersService(carRepoStup,ownerRepository,cashService);

            BuyCarInput buyCarInput = new BuyCarInput()
            { OwnerId = 10, CarId = 100, Amount = 5000};

            // Act
            string result = ownersService.BuyCar(buyCarInput);

            // Assert
            Assert.Contains("n't exist", result);
        }

        [Fact(Skip ="Working on solving error")]
        [Trait("Author","Ahmed")]
        public void BuyCar_CarWithOwner_Sold()
        {
            outputHelper.WriteLine("Test 2");

            // Arrange

            //// Create Mock of the dependencies
            //Mock<ICarsRepository> carRepoMock = new();
            //Mock<IOwnersRepository> ownersRepoMock = new();
            //Mock<ICashService> cashServiceMock = new();

            // Build the mock Data
            Car car = new Car() { Id = 10, Owner = new Owner() };

            // Setup the called method
            carRepoMock.Setup(cM=>cM.GetCarById(10)).Returns(car);

            // use the fake object as dependency
            //OwnersService ownersService = new OwnersService(carRepoMock.Object,ownersRepoMock.Object,cashServiceMock.Object);

            BuyCarInput buyCarInput = new BuyCarInput()
            {
                CarId = 10,
                OwnerId = 100,
                Amount = 5000
            };

            // Act
            string result = ownersService.BuyCar(buyCarInput);

            // Assert
            Assert.Contains("msold", result);

        }


        [Fact]
        [Trait("Author", "Ali")]
        [Trait("Priority", "5")]


        public void BuyCar_OwnorNotExist_NotExist()
        {
            outputHelper.WriteLine("Test 3");

            // Arrange
            // Build the mock Data
            Car car = new Car() { Id = 5 };
            Owner owner = null;

            // Setup the called Methods
            carRepoMock.Setup(cm => cm.GetCarById(It.IsAny<int>())).Returns(car);
            ownersRepoMock.Setup(om => om.GetOwnerById(It.IsAny<int>())).Returns(owner);

           
            BuyCarInput buyCarInput = new() { CarId = 5, OwnerId = 100, Amount = 5000 };


            // Act 
            string result = ownersService.BuyCar(buyCarInput);

            // Assert
            Assert.Contains("n't exist", result);
        }

        [Fact]
        public void BuyCar_AlreadyHaveCar()
        {
            outputHelper.WriteLine("Test 3");

            // Arrange
            // Build the mock Data
            Car car = new Car() { Id = 5 };
            Owner owner = new Owner() { Id = 100, Car = new Car()};

            // Setup the called Methods
            ownersRepoMock.Setup(cm => cm.GetOwnerById(It.IsAny<int>())).Returns(owner);
            carRepoMock.Setup(cm => cm.GetCarById(It.IsAny<int>())).Returns(car);


            // Act 
            BuyCarInput input = new() { CarId = 5, OwnerId = 100, Amount = 5000 };
            string result = ownersService.BuyCar(input);

            // Assert
            Assert.Contains("Already have car", result);
        }


        [Fact]
        public void BuyCar_LowAndInsufficient()
        {
            outputHelper.WriteLine("Test 4");

            // Arrange
            // Build the mock Data
            Car car = new Car() { Id = 5, Price = 6000 };
            Owner owner = new Owner() { Id = 100};

            // Setup the called Methods
            ownersRepoMock.Setup(cm => cm.GetOwnerById(It.IsAny<int>())).Returns(owner);
            carRepoMock.Setup(cm => cm.GetCarById(It.IsAny<int>())).Returns(car);


            // Act 
            BuyCarInput input = new() { CarId = 5, OwnerId = 100, Amount = 5000, };
            
            string result = ownersService.BuyCar(input);

            // Assert
            Assert.Contains("Insufficient", result);
        }


        [Fact]
        public void BuyCar_SuccessfulProcess()
        {
            outputHelper.WriteLine("Test 5");

            // Arrange
            // Build the mock Data
            Car car = new Car() { Id = 5, Price = 6000 };
            Owner owner = new Owner() { Id = 100 };

            // Setup the called Methods
            ownersRepoMock.Setup(cm => cm.GetOwnerById(It.IsAny<int>())).Returns(owner);
            carRepoMock.Setup(cm => cm.GetCarById(It.IsAny<int>())).Returns(car);
            carRepoMock.Setup(cm => cm.AssignToOwner(It.IsAny<int>(), It.IsAny<int>())).Returns(true);


            // Act 
            BuyCarInput input = new() { CarId = 5, OwnerId = 100, Amount = 6000, };

            string result = ownersService.BuyCar(input);

            // Assert
            Assert.Contains("Successfull", result);
        }

        [Fact]
        public void BuyCar_SomeThingWrong()
        {
            outputHelper.WriteLine("Test 5");

            // Arrange
            // Build the mock Data
            Car car = new Car() { Id = 5, Price = 6000 };
            Owner owner = new Owner() { Id = 100 };

            // Setup the called Methods
            ownersRepoMock.Setup(cm => cm.GetOwnerById(It.IsAny<int>())).Returns(owner);
            carRepoMock.Setup(cm => cm.GetCarById(It.IsAny<int>())).Returns(car);
            carRepoMock.Setup(cm => cm.AssignToOwner(It.IsAny<int>(), It.IsAny<int>())).Returns(false);


            // Act 
            BuyCarInput input = new() { CarId = 5, OwnerId = 100, Amount = 6000, };

            string result = ownersService.BuyCar(input);

            // Assert
            Assert.Contains("Something", result);
        }

    }
}
