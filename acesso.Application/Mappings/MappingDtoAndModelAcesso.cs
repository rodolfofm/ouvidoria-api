using AutoMapper;
using AutenticacaoLib.Model;
using DomainLib.Base.Entities;
using DomainLib.Base.ViewModel;
using acesso.Domain.Entities.Acesso;
using acesso.Domain.ViewModels.Acesso;
using acesso.Domain.ViewModels.Endereco;
using acesso.Domain.Entities.Endereco;

namespace acesso.Application.Mappings
{
    public class MappingDtoAndModelAcesso : Profile
    {

        public MappingDtoAndModelAcesso()
        {
            DtoToEntity();
            EntityToDto();
        }

        private void DtoToEntity()
        {
            CreateMap<EntityBaseDto, EntityBase>().ReverseMap();

            CreateMap<CidadeDto, Cidade>().ReverseMap();

            CreateMap<PermissaoDto, Permissao>().ReverseMap();
            CreateMap<UsuarioDto, Usuario>()
                .ForMember(x => x.Senha, x => x.Ignore())
                .ForMember(x => x.Cidade, x => x.Ignore());
            CreateMap<UsuarioAcessoDto, UsuarioAcesso>().ReverseMap();
            CreateMap<GrupoDto, Grupo>().ReverseMap();
            CreateMap<GrupoPermissaoDto, GrupoPermissao>().ForMember(x => x.Permissao, x => x.Ignore()).ForMember(x => x.Grupo, x => x.Ignore()).ReverseMap();
            CreateMap<GrupoPermissaoRequest, GrupoPermissao>().ForMember(x => x.Permissao, x => x.Ignore()).ForMember(x => x.Grupo, x => x.Ignore()).ReverseMap();
            CreateMap<UsuarioGrupoDto, UsuarioGrupo>().ForMember(x => x.Grupo, x => x.Ignore()).ForMember(x => x.Usuario, x => x.Ignore()).ReverseMap();
            CreateMap<UsuarioGrupoRequest, UsuarioGrupo>().ForMember(x => x.Usuario, x => x.Ignore()).ForMember(x => x.Grupo, x => x.Ignore()).ReverseMap();

        }

        private void EntityToDto()
        {
            CreateMap<Usuario, UsuarioDto>()
                .ForMember(x => x.Senha, x => x.Ignore())
                .ForMember(x => x.UsuarioGrupo, x => x.Ignore())
                ;
        }
    }
}

