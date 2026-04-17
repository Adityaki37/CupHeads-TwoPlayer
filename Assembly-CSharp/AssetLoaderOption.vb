Imports System

' Token: 0x020003A7 RID: 935
Public Class AssetLoaderOption
	' Token: 0x06000B88 RID: 2952 RVA: 0x00083F68 File Offset: 0x00082368
	Public Sub New([option] As AssetLoaderOption.Type, context As Object)
		Me.type = [option]
		Me.context = context
	End Sub

	' Token: 0x17000209 RID: 521
	' (get) Token: 0x06000B89 RID: 2953 RVA: 0x00083F7E File Offset: 0x0008237E
	' (set) Token: 0x06000B8A RID: 2954 RVA: 0x00083F86 File Offset: 0x00082386
	Public Property type As AssetLoaderOption.Type

	' Token: 0x1700020A RID: 522
	' (get) Token: 0x06000B8B RID: 2955 RVA: 0x00083F8F File Offset: 0x0008238F
	' (set) Token: 0x06000B8C RID: 2956 RVA: 0x00083F97 File Offset: 0x00082397
	Public Property context As Object

	' Token: 0x06000B8D RID: 2957 RVA: 0x00083FA0 File Offset: 0x000823A0
	Public Shared Function None() As AssetLoaderOption
		Return New AssetLoaderOption(AssetLoaderOption.Type.None, Nothing)
	End Function

	' Token: 0x06000B8E RID: 2958 RVA: 0x00083FA9 File Offset: 0x000823A9
	Public Shared Function PersistInCache() As AssetLoaderOption
		Return New AssetLoaderOption(AssetLoaderOption.Type.PersistInCache, Nothing)
	End Function

	' Token: 0x06000B8F RID: 2959 RVA: 0x00083FB2 File Offset: 0x000823B2
	Public Shared Function DontDestroyOnUnload() As AssetLoaderOption
		Return New AssetLoaderOption(AssetLoaderOption.Type.DontDestroyOnUnload, Nothing)
	End Function

	' Token: 0x06000B90 RID: 2960 RVA: 0x00083FBB File Offset: 0x000823BB
	Public Shared Function PersistInCacheTagged(tag As String) As AssetLoaderOption
		Return New AssetLoaderOption(AssetLoaderOption.Type.PersistInCacheTagged, tag)
	End Function

	' Token: 0x020003A8 RID: 936
	<Flags()>
	Public Enum Type
		' Token: 0x0400151C RID: 5404
		None = 0
		' Token: 0x0400151D RID: 5405
		PersistInCache = 1
		' Token: 0x0400151E RID: 5406
		DontDestroyOnUnload = 2
		' Token: 0x0400151F RID: 5407
		PersistInCacheTagged = 4
	End Enum
End Class
