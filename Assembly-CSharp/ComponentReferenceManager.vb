Imports System
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x0200037A RID: 890
Public Class ComponentReferenceManager
	' Token: 0x06000A5C RID: 2652 RVA: 0x0007E730 File Offset: 0x0007CB30
	Public Sub AddRef(c As Component)
		Dim text As String = String.Format("Index:<color=red>{0}</color> ComponentType:<color=red>{1}</color> GameObject:<color=red>{2}</color>", Me.count, c.[GetType]().ToString(), Me.GetGameObjectPath(c))
		Me.refs(text) = New WeakReference(c)
		Me.count += 1
	End Sub

	' Token: 0x06000A5D RID: 2653 RVA: 0x0007E788 File Offset: 0x0007CB88
	Private Function GetGameObjectPath(c As Component) As String
		Dim gameObject As GameObject = c.gameObject
		Dim text As String = "/" + gameObject.name
		While gameObject.transform.parent IsNot Nothing
			gameObject = gameObject.transform.parent.gameObject
			text = "/" + gameObject.name + text
		End While
		Return text
	End Function

	' Token: 0x06000A5E RID: 2654 RVA: 0x0007E7EC File Offset: 0x0007CBEC
	Public Sub PrintLog()
		GC.Collect()
		Global.Debug.LogError("Objects that are destroyed but still referenced:", Nothing)
		For Each keyValuePair As KeyValuePair(Of String, WeakReference) In Me.refs
			If keyValuePair.Value.IsAlive Then
				If keyValuePair.Value.Target Is Nothing Then
					Global.Debug.LogErrorFormat("Target is null {0}", New Object() { keyValuePair.Key })
				Else
					Dim component As Component = TryCast(keyValuePair.Value.Target, Component)
					If component Is Nothing Then
						Global.Debug.LogErrorFormat("Component is null {0}", New Object() { keyValuePair.Key })
					ElseIf component.gameObject Is Nothing Then
						Global.Debug.LogErrorFormat("Component attached game object is null {0}", New Object() { keyValuePair.Key })
					End If
				End If
			End If
		Next
	End Sub

	' Token: 0x04001464 RID: 5220
	Public Shared Instance As ComponentReferenceManager = New ComponentReferenceManager()

	' Token: 0x04001465 RID: 5221
	Private refs As Dictionary(Of String, WeakReference) = New Dictionary(Of String, WeakReference)()

	' Token: 0x04001466 RID: 5222
	Private count As Integer
End Class
