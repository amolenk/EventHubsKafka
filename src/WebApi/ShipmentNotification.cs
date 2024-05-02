// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.11.3
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace WebApi
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using global::Avro;
	using global::Avro.Specific;
	
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.3")]
	public partial class ShipmentNotification : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse("{\"type\":\"record\",\"name\":\"ShipmentNotification\",\"namespace\":\"WebApi\",\"fields\":[{\"n" +
				"ame\":\"order_id\",\"type\":\"string\"},{\"name\":\"location\",\"type\":\"string\"},{\"name\":\"ti" +
				"mestamp\",\"type\":\"long\"}]}");
		private string _order_id;
		private string _location;
		private long _timestamp;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return ShipmentNotification._SCHEMA;
			}
		}
		public string order_id
		{
			get
			{
				return this._order_id;
			}
			set
			{
				this._order_id = value;
			}
		}
		public string location
		{
			get
			{
				return this._location;
			}
			set
			{
				this._location = value;
			}
		}
		public long timestamp
		{
			get
			{
				return this._timestamp;
			}
			set
			{
				this._timestamp = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.order_id;
			case 1: return this.location;
			case 2: return this.timestamp;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.order_id = (System.String)fieldValue; break;
			case 1: this.location = (System.String)fieldValue; break;
			case 2: this.timestamp = (System.Int64)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
