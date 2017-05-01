namespace PDE.Solution
{
    public class CaseFactory
    {
        private readonly IProblemFactory _problemFactory;
        private readonly ITrainerFactory _trainerFactory;
        private readonly IControlPointsFactory _controlPointsFactory;

        public CaseFactory(IProblemFactory problemFacory, 
            ITrainerFactory trainFactory, 
            IControlPointsFactory controlPointsFactory)
        {
            _problemFactory = problemFacory;
            _trainerFactory = trainFactory;
            _controlPointsFactory = controlPointsFactory;
        }

        public Case Create()
        {
            var problem = _problemFactory.Create();
            return new Case(problem, _trainerFactory.Create(problem, _controlPointsFactory.Create(problem)));
        }
    }
}
