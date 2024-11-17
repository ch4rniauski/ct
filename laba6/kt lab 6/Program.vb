Public Class Token
    Public Property Type As String
    Public Property Value As String

    Public Sub New(type As String, value As String)
        Me.Type = type
        Me.Value = value
    End Sub
End Class

Public Class Lexer
    Private input As String
    Private position As Integer

    Public Sub New(input As String)
        Me.input = input
        Me.position = 0
    End Sub

    Public Function GetNextToken() As Token
        While position < input.Length
            Dim currentChar As Char = input(position)

            If Char.IsWhiteSpace(currentChar) Then
                position += 1
                Continue While
            End If

            If Char.IsLetter(currentChar) Then
                Dim start As Integer = position
                While position < input.Length AndAlso Char.IsLetter(input(position))
                    position += 1
                End While
                Dim identifier As String = input.Substring(start, position - start).ToUpper()
                If identifier = "PRINT" Then
                    Return New Token("PRINT", "PRINT")
                End If
                Return New Token("IDENTIFIER", identifier)
            End If

            If Char.IsDigit(currentChar) Then
                Dim start As Integer = position
                While position < input.Length AndAlso Char.IsDigit(input(position))
                    position += 1
                End While
                Return New Token("NUMBER", input.Substring(start, position - start))
            End If

            Select Case currentChar
                Case "="
                    position += 1
                    Return New Token("ASSIGN", "=")
                Case "+"
                    position += 1
                    Return New Token("PLUS", "+")
                Case "-"
                    position += 1
                    Return New Token("MINUS", "-")
                Case "*"
                    position += 1
                    Return New Token("MULTIPLY", "*")
                Case "/"
                    position += 1
                    Return New Token("DIVIDE", "/")
                Case Else
                    Throw New Exception($"Unexpected character: {currentChar}")
            End Select
        End While

        Return New Token("EOF", "")
    End Function
End Class

Public Class Parser
    Private lexer As Lexer
    Private currentToken As Token
    Private variables As New Dictionary(Of String, Integer)

    Public Sub New(lexer As Lexer)
        Me.lexer = lexer
        currentToken = lexer.GetNextToken()
    End Sub

    Private Sub Eat(tokenType As String)
        If currentToken.Type = tokenType Then
            currentToken = lexer.GetNextToken()
        Else
            Throw New Exception($"Unexpected token: {currentToken.Type} with value '{currentToken.Value}'")
        End If
    End Sub

    Public Sub Execute()
        While currentToken.Type <> "EOF"
            If currentToken.Type = "PRINT" Then
                Eat("PRINT")
                Dim value As Integer = Expression()
                Console.WriteLine(value)
            ElseIf currentToken.Type = "IDENTIFIER" Then
                Dim varName As String = currentToken.Value
                Eat("IDENTIFIER")
                Eat("ASSIGN")
                Dim value As Integer = Expression()
                variables(varName) = value
            Else
                Throw New Exception("Invalid command")
            End If
        End While
    End Sub

    Private Function Expression() As Integer
        Dim result As Integer = Term()

        While currentToken.Type = "PLUS" OrElse currentToken.Type = "MINUS"
            Dim op As String = currentToken.Value
            If op = "+" Then
                Eat("PLUS")
                result += Term()
            ElseIf op = "-" Then
                Eat("MINUS")
                result -= Term()
            End If
        End While

        Return result
    End Function

    Private Function Term() As Integer
        Dim result As Integer = Factor()

        While currentToken.Type = "MULTIPLY" OrElse currentToken.Type = "DIVIDE"
            Dim op As String = currentToken.Value
            If op = "*" Then
                Eat("MULTIPLY")
                result *= Factor()
            ElseIf op = "/" Then
                Eat("DIVIDE")
                result \= Factor() ' Целочисленное деление
            End If
        End While

        Return result
    End Function

    Private Function Factor() As Integer
        Dim token As Token = currentToken
        If token.Type = "NUMBER" Then
            Eat("NUMBER")
            Return Integer.Parse(token.Value)
        ElseIf token.Type = "IDENTIFIER" Then
            Dim varName As String = token.Value
            Eat("IDENTIFIER")
            If variables.ContainsKey(varName) Then
                Return variables(varName)
            Else
                Throw New Exception($"Undefined variable: {varName}")
            End If
        End If

        Throw New Exception("Invalid factor")
    End Function
End Class

Module Module1
    Sub Main()
        Console.WriteLine("Enter your BASIC program (type 'END' to finish):")
        Dim input As New Text.StringBuilder()
        Dim line As String

        ' Чтение программы
        Do
            line = Console.ReadLine()
            If line.Trim().ToUpper() = "END" Then Exit Do
            input.AppendLine(line)
        Loop

        Dim lexer As New Lexer(input.ToString())
        Dim parser As New Parser(lexer)

        Try
            parser.Execute()
        Catch ex As Exception
            Console.WriteLine($"Error: {ex.Message}")
        End Try

        Console.WriteLine("Press any key to exit...")
        Console.ReadKey()
    End Sub
End Module