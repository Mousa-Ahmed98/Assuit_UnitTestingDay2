using CarAPI.Entities;
using CarFactoryAPI.Entities;
using CarFactoryAPI.Repositories_DAL;
using Moq;
using Moq.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarFactoryAPI_test
{
    public class CarRepositoryTests
    {
        private Mock<FactoryContext> factoryContextMock;
        private CarRepository carRepository;
        private readonly OwnerRepository ownerRepository;
        public CarRepositoryTests()
        {
            // Create Mock of dependencies
            factoryContextMock = new Mock<FactoryContext>();

            // use fake object as dependency
            carRepository = new CarRepository(factoryContextMock.Object);

            ownerRepository = new OwnerRepository(factoryContextMock.Object);
        }
        [Fact]
        [Trait("Author", "Mousa")]
        [Trait("Priority", "9")]

        public void GetCarById_AskForCar10_ReturnCar()
        {
            // Arrange

            // Build the mock Data
            List<Car> cars = new List<Car>() { new Car() { Id = 10 } } ;

            // setup called DbSets
            factoryContextMock.Setup(fcm=>fcm.Cars).ReturnsDbSet(cars);

            // Act 
            Car car = carRepository.GetCarById(10);

            // Aassert

            Assert.NotNull(car);
        }


        [Fact]
        [Trait("Author", "Mousa")]
        [Trait("Priority", "1")]

        public void GetOwnerWithId_OwnerId100()
        {
            // Arrange

            // Build the mock Data
            List<Owner> owners = new List<Owner>() { { new Owner() { Id = 100 } } };

            // setup called DbSets
            factoryContextMock.Setup(fcm => fcm.Owners).ReturnsDbSet(owners);

            // Act 
            
            Owner owner = ownerRepository.GetOwnerById(100);

            // Aassert

            Assert.NotNull(owner);
        }

        [Fact]
        [Trait("Author", "Mousa")]
        [Trait("Priority", "2")]

        public void GetListOfOwners()
        {
            // Arrange

            // Build the mock Data
            List<Owner> owners = new List<Owner>() { { new Owner() { Id = 100 } }, new Owner() { Id = 200 } };

            // setup called DbSets
            factoryContextMock.Setup(fcm => fcm.Owners).ReturnsDbSet(owners);

            // Act 

            List<Owner>  ListOfOwners = ownerRepository.GetAllOwners();

            // Aassert

            Assert.NotNull(ListOfOwners);
        }
    }
}
