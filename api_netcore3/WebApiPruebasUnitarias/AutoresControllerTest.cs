using api_netcore3.Contexts;
using api_netcore3.Controllers;
using api_netcore3.Entities;
using api_netcore3.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace WebApiPruebasUnitarias
{

    [TestClass]
    public class AutoresControllerTests
    {
        //[TestMethod]
        //public void Get_SiElAutorNoExiste_SeNosRetornaUn404()
        //{
        //    // preparación
        //    var idAutor = 1;
        //    var mock = new Mock<IRepositorioAutores>();
        //    mock.Setup(x => x.ObtenerPorId(idAutor)).Returns(default(Autor));
        //    var autoresController = new AutoresController(mock.Object);

        //    // prueba
        //    var resultado = autoresController.Get(idAutor);

        //    // Verificación
        //    Assert.IsInstanceOfType(resultado.Result, typeof(NotFoundResult));
        //}

        //[TestMethod]
        //public void Get_SiElAutorExiste_SeNosRetornaElAutor()
        //{
        //    // Preparación
        //    var autorMock = new Autor()
        //    {
        //        Id = 1,
        //        Nombre = "Felipe Gavilan"
        //    };
        //    var mock = new Mock<IRepositorioAutores>();
        //    mock.Setup(x => x.ObtenerPorId(autorMock.Id)).Returns(autorMock);
        //    var autoresController = new AutoresController(mock.Object);

        //    // Prueba
        //    var resultado = autoresController.Get(autorMock.Id);

        //    // Verificación
        //    Assert.IsNotNull(resultado.Value);
        //    Assert.AreEqual(resultado.Value.Id, autorMock.Id);
        //    Assert.AreEqual(resultado.Value.Nombre, autorMock.Nombre);
        //}

    }
}
