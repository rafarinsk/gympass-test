Solução
========
Resultado gerado para o arquivo de log proposto

```text
Posiçao Chegada Código Piloto   Nome Piloto     Qtde Voltas Completadas Tempo Total de Prova    Melhor Volta    Tempo Melhor Volta      Velocidade Média da Corrida     Diferença de tempo para o vencedor
1       038     F.MASSA 4       4:11.578        3       1:02.769        44,246  0:00.000
2       002     K.RAIKKONEN     4       4:15.153        4       1:03.076        43,626  0:05.117
3       033     R.BARRICHELLO   4       4:16.080        3       1:03.716        43,468  0:05.583
4       023     M.WEBBER        4       4:17.722        4       1:04.216        43,191  0:08.972
5       015     F.ALONSO        4       4:54.221        2       1:07.011        37,833  0:49.738
6       011     S.VETTEL        2       5:09.179        2       1:37.864        18,001  1:22.657

Melhor volta da corrida:
Piloto  Nº Volta        Tempo Volta     Velocidade média da volta
038 - F.MASSA   3       1:02.769        44,334
```

Como executar
--------
O projeto principal foi feito em .Net Core 2.1.

As bibliotecas foram feitas em .Net Standard 2.0

O projeto de testes é um NUnit e para não utilizar bibliotecas de terceiros, não está feito com mocks mas sim com as implementações dos projetos.

Para compilar, executar em linha de comando, na pasta da solução, em uma máquina capaz de compilar soluções .Net:
```text
dotnet msbuild
```
Para executar o resultado gerador, ir para a pasta output\Debug\netcoreapp2.1 e executar:
```
dotnet gympass-test.dll racelog.txt
```
