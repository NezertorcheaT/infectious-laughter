namespace Saving
{
    /// <summary>
    /// это штука, которая может сохранять хуйни
    /// </summary>
    /// <typeparam name="T">тип, в котором будет сохраняться объект, например <code>string</code></typeparam>
    public interface IFileSaver<T>
    {
        /// <summary>
        /// хУйня, которую может сщхранять <code>IFileSaver</code>
        /// </summary>
        public interface ISavable
        {
            /// <summary>
            /// произвезти превращение в тип <c>T</c>
            /// </summary>
            /// <returns>объект типа <c>T</c>, должна быть возможность обратного превращения с помощью <c>ISavable.Deconvert()</c></returns>
            T Convert();

            /// <summary>
            /// произвезти обратное превращение объекта типа <c>T</c> в ISavable
            /// </summary>
            /// <param name="converted">объект типа <c>T</c> для обратного превращения</param>
            /// <param name="saver">штука, которая может сохранять хуйни типа <c>T</c></param>
            /// <returns></returns>
            ISavable Deconvert(T converted, IFileSaver<T> saver);
        }

        /// <summary>
        /// сохранить объект ISavable куда-то, например на диск или в облако хз
        /// </summary>
        /// <param name="savable">хуйня для сохранения</param>
        void Save(ISavable savable);

        /// <summary>
        /// получит объект типа <c>T</c> с диска или облака хз
        /// </summary>
        /// <param name="path">путь до объекта типа <c>T</c>, хз может кому надо</param>
        /// <returns>объект типа <c>T</c></returns>
        T Read(string path);
    }
}