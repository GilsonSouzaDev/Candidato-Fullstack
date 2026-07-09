# RestAPI-Candidato

Repositorio destinado aos projetos de back end em c# com .NET

Projeto Candidato

Este projeto API REST em C# utiliza os padrões MVC (Model-View-Controller), DAO (Data Access Object), fachada e strategy. O objetivo do projeto é criar um CRUD (Create, Read, Update, Delete) para um candidato a cursos.

O modelo principal do projeto é a classe "Candidatoo", que possui os seguintes atributos:

- Id (long): representa o identificador único do candidato.
- Nome (string): representa o nome do candidato.
- FiliacaoId (long): representa o identificador único da filiação do candidato.
- Filiacao (Filiacao): representa a associação com a filiação do candidato. Essa propriedade é virtual e permite o acesso à entidade de filiação.
- EnderecoId (long): representa o identificador único do endereço do candidato.
- Endereco (Endereco): representa a associação com o endereço do candidato. Essa propriedade é virtual e permite o acesso à entidade de endereço.
- Telefones (ICollection<Telefone>): representa uma coleção de telefones associados ao candidato.
- Cursos (ICollection<Curso>): representa uma coleção de cursos associados ao candidato.

Essa classe utiliza o pacote Newtonsoft.Json para realizar a serialização e desserialização de objetos JSON. Também faz uso das anotações System.ComponentModel.DataAnnotations e System.ComponentModel.DataAnnotations.Schema para definir metadados de validação e mapeamento de dados no banco de dados, respectivamente.

O projeto segue a arquitetura MVC, onde o modelo (representado pela classe Candidatoo) contém as regras de negócio e os dados, a view é responsável pela apresentação dos dados e a controller processa as requisições e coordena as interações entre o modelo e a view. O padrão DAO é utilizado para isolar a lógica de acesso aos dados, proporcionando uma abstração para as operações de criação, leitura, atualização e exclusão no banco de dados. A fachada é utilizada para simplificar o acesso às funcionalidades do sistema, fornecendo métodos de alto nível para interações com o CRUD do candidato. O padrão strategy é empregado para permitir a troca flexível de algoritmos ou comportamentos relacionados a determinadas operações, como validação de dados ou cálculos específicos.

Em resumo, este projeto API REST em C# com os padrões MVC, DAO, fachada e strategy visa criar um sistema de CRUD para gerenciar candidatos a cursos, fornecendo uma API para criar, ler, atualizar e excluir candidatos, bem como realizar operações relacionadas a suas filiações, endereços, telefones e cursos associados.
