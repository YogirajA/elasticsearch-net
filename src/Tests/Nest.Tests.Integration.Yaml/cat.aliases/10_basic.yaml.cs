using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using NUnit.Framework;
using Nest.Tests.Integration.Yaml;


namespace Nest.Tests.Integration.Yaml.CatAliases1
{
	public partial class CatAliases1YamlTests
	{	


		[NCrunch.Framework.ExclusivelyUses("ElasticsearchYamlTests")]
		public class Setup1Tests : YamlTestsBase
		{
			[Test]
			public void Setup1Test()
			{	

			}
		}

		[NCrunch.Framework.ExclusivelyUses("ElasticsearchYamlTests")]
		public class Help2Tests : YamlTestsBase
		{
			[Test]
			public void Help2Test()
			{	

				//do cat.aliases 
				this.Do(()=> this._client.CatAliasesGet(nv=>nv
					.Add("help", @"true")
				));

				//match this._status: 
				this.IsMatch(this._status, @"/^  alias            .+   \n
    index            .+   \n
    filter           .+   \n
    routing.index    .+   \n
    routing.search   .+   \n
$/
");

			}
		}

		[NCrunch.Framework.ExclusivelyUses("ElasticsearchYamlTests")]
		public class EmptyCluster3Tests : YamlTestsBase
		{
			[Test]
			public void EmptyCluster3Test()
			{	

				//do cat.aliases 
				this.Do(()=> this._client.CatAliasesGet());

				//match this._status: 
				this.IsMatch(this._status, @"/^ $/
");

			}
		}

		[NCrunch.Framework.ExclusivelyUses("ElasticsearchYamlTests")]
		public class SimpleAlias4Tests : YamlTestsBase
		{
			[Test]
			public void SimpleAlias4Test()
			{	

				//do indices.create 
				this.Do(()=> this._client.IndicesCreatePut("test", null));

				//do indices.put_alias 
				this.Do(()=> this._client.IndicesPutAlias("test", "test_alias", null));

				//do cat.aliases 
				this.Do(()=> this._client.CatAliasesGet());

				//match this._status: 
				this.IsMatch(this._status, @"/^
    test_alias          \s+
    test                \s+
    -                   \s+
    -                   \s+
    -                   \s+
$/
");

			}
		}

		[NCrunch.Framework.ExclusivelyUses("ElasticsearchYamlTests")]
		public class ComplexAlias5Tests : YamlTestsBase
		{
			[Test]
			public void ComplexAlias5Test()
			{	

				//do indices.create 
				this.Do(()=> this._client.IndicesCreatePut("test", null));

				//do indices.put_alias 
				_body = new {
					index_routing= "ir",
					search_routing= "sr1,sr2",
					filter= new {
						term= new {
							foo= "bar"
						}
					}
				};
				this.Do(()=> this._client.IndicesPutAlias("test", "test_alias", _body));

				//do cat.aliases 
				this.Do(()=> this._client.CatAliasesGet());

				//match this._status: 
				this.IsMatch(this._status, @"/^
    test_alias          \s+
    test                \s+
    [*]                 \s+
    ir                  \s+
    sr1,sr2             \s+
$/
");

			}
		}

		[NCrunch.Framework.ExclusivelyUses("ElasticsearchYamlTests")]
		public class AliasName6Tests : YamlTestsBase
		{
			[Test]
			public void AliasName6Test()
			{	

				//do indices.create 
				this.Do(()=> this._client.IndicesCreatePut("test", null));

				//do indices.put_alias 
				this.Do(()=> this._client.IndicesPutAlias("test", "test_1", null));

				//do indices.put_alias 
				this.Do(()=> this._client.IndicesPutAlias("test", "test_2", null));

				//do cat.aliases 
				this.Do(()=> this._client.CatAliasesGet("test_1"));

				//match this._status: 
				this.IsMatch(this._status, @"/^test_1 .+ \n$/");

				//do cat.aliases 
				this.Do(()=> this._client.CatAliasesGet("test_2"));

				//match this._status: 
				this.IsMatch(this._status, @"/^test_2 .+ \n$/");

				//do cat.aliases 
				this.Do(()=> this._client.CatAliasesGet("test_*"));

				//match this._status: 
				this.IsMatch(this._status, @"/ (^|\n)test_1 .+ \n/");

				//match this._status: 
				this.IsMatch(this._status, @"/ (^|\n)test_2 .+ \n/");

			}
		}

		[NCrunch.Framework.ExclusivelyUses("ElasticsearchYamlTests")]
		public class ColumnHeaders7Tests : YamlTestsBase
		{
			[Test]
			public void ColumnHeaders7Test()
			{	

				//do indices.create 
				this.Do(()=> this._client.IndicesCreatePut("test", null));

				//do indices.put_alias 
				this.Do(()=> this._client.IndicesPutAlias("test", "test_1", null));

				//do cat.aliases 
				this.Do(()=> this._client.CatAliasesGet(nv=>nv
					.Add("v", @"true")
				));

				//match this._status: 
				this.IsMatch(this._status, @"/^  alias           \s+
    index           \s+
    filter          \s+
    routing.index   \s+
    routing.search  \s+
    \n
    test_1          \s+
    test            \s+
    -               \s+
    -               \s+
    -               \s+
$/
");

			}
		}

		[NCrunch.Framework.ExclusivelyUses("ElasticsearchYamlTests")]
		public class SelectColumns8Tests : YamlTestsBase
		{
			[Test]
			public void SelectColumns8Test()
			{	

				//do indices.create 
				this.Do(()=> this._client.IndicesCreatePut("test", null));

				//do indices.put_alias 
				this.Do(()=> this._client.IndicesPutAlias("test", "test_1", null));

				//do cat.aliases 
				this.Do(()=> this._client.CatAliasesGet(nv=>nv
					.Add("h", new [] {
						@"index",
						@"alias"
					})
				));

				//match this._status: 
				this.IsMatch(this._status, @"/^ test \s+ test_1 \s+ $/");

				//do cat.aliases 
				this.Do(()=> this._client.CatAliasesGet(nv=>nv
					.Add("h", new [] {
						@"index",
						@"alias"
					})
					.Add("v", @"true")
				));

				//match this._status: 
				this.IsMatch(this._status, @"/^
    index \s+ alias  \s+ \n
    test  \s+ test_1 \s+ \n
$/
");

			}
		}
	}
}
