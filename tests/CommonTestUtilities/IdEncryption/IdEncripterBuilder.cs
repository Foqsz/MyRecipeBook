using Sqids;

namespace CommonTestUtilities.IdEncryption;
public class IdEncripterBuilder
{
    public static SqidsEncoder<long> Build()
    {
        return new SqidsEncoder<long>(new()
        {
            MinLength = 3,
            Alphabet = "lf9aw5VgG6YoekSvOWhH2qi1NDpjQzKT83uE4IUtMCRZ7yxrsL0nPcAFmdbBXJ",
        });
    }
}
