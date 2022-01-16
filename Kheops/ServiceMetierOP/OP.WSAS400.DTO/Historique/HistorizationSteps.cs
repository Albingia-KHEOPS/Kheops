using System;

namespace OP.WSAS400.DTO {
    [Flags]
    public enum HistorizationSteps {
        InitializeState = 0x0,
        CheckHistoAlreadyDone = 0x1,
        ResetArchive = 0x2,
        ArchiveAffaire = 0x4,
        ArchiveExtraTables = 0x8,
        ResetClauses = 0x10,
        UpdateControleEtape = 0x20,
        CreateOrUpdateAvenant = 0x40,
        ReleaseGaranties = 0x80,
        DeleteEcheanciers = 0x100,
        DeleteClausesRegularisation = 0x200,
        DeletePrimes = 0x400,
        DeleteTempWordDocuments = 0x800,
        CheckFolderState = 0x1000
    }
}
