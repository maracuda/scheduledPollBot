using BusinessLogic;

using FluentAssertions;

using NUnit.Framework;

namespace UnitTests
{
    public class PostgreUrlParserTests
    {
        [Test]
        public void Parse_Should_Return_Exact_Object()
        {
            var url =
                "postgres://aaa:bbb@cccc:5432/aas";
            var expected = new ConnectionProperties()
                {
                    UserName = "aaa",
                    Password = "bbb",
                    HostAndPort = "cccc:5432",
                    DatabaseName = "aas",
                };
            
            var actual = new PostgreUrlParser().Parse(url);

            actual.Should().BeEquivalentTo(expected);
        }
    }
}