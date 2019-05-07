namespace gympass_test.IO
{
    public static class RaceLogStructure
    {
        public const int FieldCount = 5;
        public const char FieldSeparator = '\t';
        public const int FinishTimeIndex = 0;
        public const string FinishTimeFormat = "h\\:mm\\:ss\\.fff";
        public const int Pilot = 1;
        public const int Lap = 2;
        public const int Duration = 3;
        public const string DurationFormat = "m\\:ss\\.fff";
        public const int AverageSpeed = 4;
        public const string Culture = "pt-BR";
        public const bool HasColumnHeaders = true;
        public const int RaceLaps = 4;
    }

}
