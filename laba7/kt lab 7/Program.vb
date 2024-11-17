Imports System.Text.RegularExpressions

Public Class CodeAnalyzer
    Private input As String
    Private variables As New HashSet(Of String)()
    Private usedVariables As New HashSet(Of String)()
    Private issues As New List(Of String)()

    Public Sub New(input As String)
        Me.input = input
    End Sub

    Public Function Analyze() As List(Of String)
        Dim lines() As String = input.Split(New String() {Environment.NewLine}, StringSplitOptions.None)

        For Each line As String In lines
            CheckForEval(line)
            CheckVariableInitialization(line)
            CheckForErrorHandling(line)
        Next

        Return issues
    End Function

    Private Sub CheckForEval(line As String)
        If line.Contains("eval") Then
            issues.Add($"Potential vulnerability found: 'eval' used in line: {line}")
        End If
    End Sub

    Private Sub CheckVariableInitialization(line As String)
        Dim variablePattern As String = "\b([a-zA-Z_][a-zA-Z0-9_]*)\s*="
        Dim variableMatches As MatchCollection = Regex.Matches(line, variablePattern)

        For Each match As Match In variableMatches
            variables.Add(match.Groups(1).Value)
        Next

        Dim usedVariablePattern As String = "\b([a-zA-Z_][a-zA-Z0-9_]*)\b"
        Dim usedMatches As MatchCollection = Regex.Matches(line, usedVariablePattern)

        For Each match As Match In usedMatches
            If Not variables.Contains(match.Value) AndAlso Not usedVariables.Contains(match.Value) Then
                issues.Add($"Warning: Variable '{match.Value}' used before initialization in line: {line}")
            End If
            usedVariables.Add(match.Value)
        Next
    End Sub

    Private Sub CheckForErrorHandling(line As String)
        If line.Contains("try") OrElse line.Contains("catch") OrElse line.Contains("finally") Then
            ' Предполагаем, что обработка ошибок присутствует
        ElseIf line.Contains("throw") OrElse line.Contains("raise") Then
            issues.Add($"Error handling not present in line: {line}")
        End If
    End Sub
End Class

Module Module1
    Sub Main()
        Console.WriteLine("Напишите код (введите 'END', чтобы завершить:")
        Dim input As New Text.StringBuilder()
        Dim line As String

        ' Чтение кода
        Do
            line = Console.ReadLine()
            If line.Trim().ToUpper() = "END" Then Exit Do
            input.AppendLine(line)
        Loop

        Dim analyzer As New CodeAnalyzer(input.ToString())
        Dim issues As List(Of String) = analyzer.Analyze()

        If issues.Count = 0 Then
            Console.WriteLine("Ошибок не было найдено.")
        Else
            Console.WriteLine("Найденные ошибки:")
            For Each issue As String In issues
                Console.WriteLine(issue)
            Next
        End If

        Console.WriteLine("Press any key to exit...")
        Console.ReadKey()
    End Sub
End Module