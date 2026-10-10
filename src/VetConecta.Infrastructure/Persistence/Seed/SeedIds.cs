namespace VetConecta.Infrastructure.Persistence.Seed;

/// <summary>
/// Ids y fecha fijos para los datos de prueba sembrados via HasData.
/// EF Core exige valores constantes (no Guid.NewGuid() ni DateTime.UtcNow)
/// porque compara el modelo entre migraciones para detectar cambios.
/// </summary>
public static class SeedIds
{
    public static readonly DateTime FechaSemilla = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static readonly Guid PropietarioDemoId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    public static readonly Guid PersonalDemoId = Guid.Parse("22222222-2222-2222-2222-222222222222");

    public static readonly Guid MascotaColitaId = Guid.Parse("33333333-3333-3333-3333-333333333333");
    public static readonly Guid MascotaLunaId = Guid.Parse("44444444-4444-4444-4444-444444444444");
    public static readonly Guid MascotaSimbaId = Guid.Parse("55555555-5555-5555-5555-555555555555");

    public static readonly Guid VacunaSextupleId = Guid.Parse("66666666-6666-6666-6666-666666666601");
    public static readonly Guid VacunaAntirrabicaId = Guid.Parse("66666666-6666-6666-6666-666666666602");
    public static readonly Guid VacunaBordetellaId = Guid.Parse("66666666-6666-6666-6666-666666666603");

    public static readonly Guid VisitaColitaId = Guid.Parse("77777777-7777-7777-7777-777777777701");

    public static readonly Guid RecetaLunaId = Guid.Parse("88888888-8888-8888-8888-888888888801");
    public static readonly Guid RecetaColitaId = Guid.Parse("88888888-8888-8888-8888-888888888802");
    public static readonly Guid MedAmoxicilinaId = Guid.Parse("88888888-8888-8888-8888-888888888811");
    public static readonly Guid MedMeloxicamSuspId = Guid.Parse("88888888-8888-8888-8888-888888888812");
    public static readonly Guid MedMeloxicam2mgId = Guid.Parse("88888888-8888-8888-8888-888888888813");

    public static readonly Guid InternamientoColitaId = Guid.Parse("99999999-9999-9999-9999-999999999901");
    public static readonly Guid TratamientoCefalexinaId = Guid.Parse("99999999-9999-9999-9999-999999999911");
    public static readonly Guid TratamientoMeloxicamId = Guid.Parse("99999999-9999-9999-9999-999999999912");
    public static readonly Guid TratamientoFluidoterapiaId = Guid.Parse("99999999-9999-9999-9999-999999999913");
    public static readonly Guid EvolucionColitaId = Guid.Parse("99999999-9999-9999-9999-999999999921");

    public static readonly Guid ProductoNexgardId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa01");
    public static readonly Guid ProductoShampooId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa02");
    public static readonly Guid ProductoRoyalCaninId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa03");
    public static readonly Guid ProductoAmoxicilinaId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa04");
    public static readonly Guid ProductoMeloxicamId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa05");
}
