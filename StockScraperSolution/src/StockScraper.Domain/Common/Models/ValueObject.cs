﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScraper.Domain.Common.Models
{
    public abstract class ValueObject
    {
        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType()) return false;
            var other = (ValueObject)obj;
            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public override int GetHashCode() =>
            GetEqualityComponents().Aggregate(0, (hash, obj) => hash ^ obj.GetHashCode());

        public static bool operator ==(ValueObject a, ValueObject b) =>
            a?.Equals(b) ?? b is null;

        public static bool operator !=(ValueObject a, ValueObject b) =>
            !(a == b);
    }

}
