using System.Collections.Generic;
using System.Threading.Tasks;
using AidaTemplate.Api.Model;
using Core.Http.Filtering;
using Core.Http.Filtering.Extensions;

namespace AidaTemplate.Api.UseCases {
    public class GetSampleQueryHandler {

        private readonly List<Sample> samples = new List<Sample>() {
            new Sample() { Id = "1", Name = "FirstSample", Description = "First sample description"},
            new Sample() { Id = "2", Name = "SecondSample", Description = "Second sample description"},
            new Sample() { Id = "3", Name = "ThirdSample", Description = "Third sample description"}
        };

        public Task<PagedResponse<Sample>> Execute(Query<SampleFilter> query) {
            var response = samples
                            .ApplyQuery(query)
                            .ToPagedResponse(query.PageNumber, query.PageSize);
            return Task.FromResult(response);
        }
    }
}