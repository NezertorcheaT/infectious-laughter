namespace Entity.Relationships
{
    public abstract class Fraction
    {
        public enum Relation
        {
            Hostile,
            Natural,
            Friendly
        }

        public abstract Relation GetRelation(Fraction fraction);
    }
}