using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace Shared.Extensions;

public static class AutoMapperExtensions
{
    public static void EnsureAllPropertiesAreMapped(this IConfigurationProvider context)
    {
        var badTypeMaps = context.Internal().
            GetAllTypeMaps().
            Where(x => x.SourceType != x.DestinationType).
            SelectMany(x => x.MemberMaps).
            Where(x => x.CanResolveValue && x.Ignored).
            GroupBy(x => x.TypeMap).
            OrderBy(x => x.Key.DestinationType.Name).
            Select(x => new AutoMapperConfigurationException.TypeMapConfigErrors(x.Key,
                x.Select(z => z.DestinationName).OrderBy(z => z).ToArray(), false)).
            ToArray();

        if (badTypeMaps.Any())
        {
            throw new AutoMapperConfigurationException(badTypeMaps);
        }
    }
}
