Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Text
Imports UnityEngine
Imports UnityEngine.U2D

' Token: 0x02000B37 RID: 2871
Public Class DEBUG_AssetPrinter
	Inherits MonoBehaviour

	' Token: 0x0600459C RID: 17820 RVA: 0x0024B902 File Offset: 0x00249D02
	Private Sub Awake()
		Global.UnityEngine.[Object].DontDestroyOnLoad(MyBase.gameObject)
	End Sub

	' Token: 0x0600459D RID: 17821 RVA: 0x0024B910 File Offset: 0x00249D10
	Private Sub OnGUI()
		Dim guistyle As GUIStyle = New GUIStyle(GUI.skin.GetStyle("Box"))
		guistyle.alignment = TextAnchor.UpperLeft
		Dim stringBuilder As StringBuilder = New StringBuilder()
		stringBuilder.AppendLine("Load Operations: " + AssetBundleLoader.loadCounter)
		Dim list As IList = AssetBundleLoader.DEBUG_LoadedAssetBundles()
		stringBuilder.AppendFormat("=== AssetBundles ({0}) ===" & vbLf, list.Count)
		Dim enumerator As IEnumerator = list.GetEnumerator()
		Try
			While enumerator.MoveNext()
				Dim obj As Object = enumerator.Current
				Dim text As String = CStr(obj)
				stringBuilder.AppendLine(text)
			End While
		Finally
			Dim disposable As IDisposable = TryCast(enumerator, IDisposable)
			Dim disposable2 As IDisposable = disposable
			If disposable IsNot Nothing Then
				disposable2.Dispose()
			End If
		End Try
		Dim allLoadedAssetBundles As IEnumerable(Of AssetBundle) = AssetBundle.GetAllLoadedAssetBundles()
		Dim num As Integer = 0
		For Each assetBundle As AssetBundle In AssetBundle.GetAllLoadedAssetBundles()
			num += 1
		Next
		stringBuilder.AppendFormat("=== System AssetBundles ({0}) ===" & vbLf, num)
		For Each assetBundle2 As AssetBundle In AssetBundle.GetAllLoadedAssetBundles()
			stringBuilder.AppendLine(assetBundle2.name)
		Next
		GUI.Box(New Rect(0F, 0F, 400F, CSng(Screen.height)), stringBuilder.ToString())
		stringBuilder.Length = 0
		list = AssetLoader(Of SpriteAtlas).DEBUG_GetLoadedAssets()
		stringBuilder.AppendFormat("=== Cached SpriteAtlases ({0}) ===" & vbLf, list.Count)
		Dim enumerator4 As IEnumerator = list.GetEnumerator()
		Try
			While enumerator4.MoveNext()
				Dim obj2 As Object = enumerator4.Current
				Dim text2 As String = CStr(obj2)
				stringBuilder.AppendLine(text2)
			End While
		Finally
			Dim disposable3 As IDisposable = TryCast(enumerator4, IDisposable)
			Dim disposable4 As IDisposable = disposable3
			If disposable3 IsNot Nothing Then
				disposable4.Dispose()
			End If
		End Try
		list = AssetLoader(Of AudioClip).DEBUG_GetLoadedAssets()
		stringBuilder.AppendFormat("=== Cached Music ({0}) ===" & vbLf, list.Count)
		Dim enumerator5 As IEnumerator = list.GetEnumerator()
		Try
			While enumerator5.MoveNext()
				Dim obj3 As Object = enumerator5.Current
				Dim text3 As String = CStr(obj3)
				stringBuilder.AppendLine(text3)
			End While
		Finally
			Dim disposable5 As IDisposable = TryCast(enumerator5, IDisposable)
			Dim disposable6 As IDisposable = disposable5
			If disposable5 IsNot Nothing Then
				disposable6.Dispose()
			End If
		End Try
		list = AssetLoader(Of Texture2D()).DEBUG_GetLoadedAssets()
		stringBuilder.AppendFormat("=== Cached Textures ({0}) ===" & vbLf, list.Count)
		Dim enumerator6 As IEnumerator = list.GetEnumerator()
		Try
			While enumerator6.MoveNext()
				Dim obj4 As Object = enumerator6.Current
				Dim text4 As String = CStr(obj4)
				stringBuilder.AppendLine(text4)
			End While
		Finally
			Dim disposable7 As IDisposable = TryCast(enumerator6, IDisposable)
			Dim disposable8 As IDisposable = disposable7
			If disposable7 IsNot Nothing Then
				disposable8.Dispose()
			End If
		End Try
		GUI.Box(New Rect(400F, 0F, 400F, CSng(Screen.height)), stringBuilder.ToString())
		stringBuilder.Length = 0
		list = Resources.FindObjectsOfTypeAll(Of SpriteAtlas)()
		stringBuilder.AppendFormat("=== System SpriteAtlases ({0}) ===" & vbLf, list.Count)
		Dim enumerator7 As IEnumerator = list.GetEnumerator()
		Try
			While enumerator7.MoveNext()
				Dim obj5 As Object = enumerator7.Current
				stringBuilder.AppendLine(CType(obj5, SpriteAtlas).name)
			End While
		Finally
			Dim disposable9 As IDisposable = TryCast(enumerator7, IDisposable)
			Dim disposable10 As IDisposable = disposable9
			If disposable9 IsNot Nothing Then
				disposable10.Dispose()
			End If
		End Try
		GUI.Box(New Rect(800F, 0F, 400F, CSng(Screen.height)), stringBuilder.ToString())
	End Sub
End Class
