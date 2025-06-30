using Newtonsoft.Json;

namespace GoodsShopper.Domain.DTO.Character
{
    /// <summary>
    /// 角色新增DTO
    /// </summary>
    public class CharacterInsertDto
    {

        /// <summary>
        /// 角色名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => JsonConvert.SerializeObject(this);
    }
}
