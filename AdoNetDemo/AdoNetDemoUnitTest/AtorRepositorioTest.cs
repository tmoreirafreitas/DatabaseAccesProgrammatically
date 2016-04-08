using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using SVD.Model;
using AdoNetDemo;
using System.Collections.Generic;

namespace AdoNetDemoUnitTest
{
    [TestClass]
    public class AtorRepositorioTest
    {
        Ator a;
        [TestMethod]
        public void InsertAtorTest()
        {
            var factory = new MockRepository(MockBehavior.Loose);
            var repositorio = factory.Create<IRepositorio<Ator>>();
            var atorRepo = repositorio.Object;
            
            //repositorio.Setup(t => t.Insert(new Ator())).Returns(It.IsAny<int>);
            //factory.Verify();
        }

        [TestMethod]
        public void GetByTest()
        {
            var factory = new MockRepository(MockBehavior.Loose);
            var atorRepositorio = factory.Create<IRepositorio<Ator>>();
            atorRepositorio.Setup(t => t.GetBy(It.IsAny<int>())).Returns(It.IsAny<Ator>());
        }

        [TestMethod]
        public void GetAllTest()
        {
            var factory = new MockRepository(MockBehavior.Loose);
            var atorRepositorio = factory.Create<IRepositorio<Ator>>();
            atorRepositorio.Setup(t => t.GetAll()).Returns(It.IsAny<List<Ator>>);
        }
    }
}
