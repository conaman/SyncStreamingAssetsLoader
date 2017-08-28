using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SyncStreamingAssetsLoader {
#if !UNITY_EDITOR && UNITY_ANDROID
	private struct Entry {
		public Entry(long index, long size) {
			this.index = index;
			this.size = size;
		}
		public long index;
		public long size;
	}

	private Dictionary<string, Entry> _entries = new Dictionary<string, Entry>(StringComparer.OrdinalIgnoreCase);
	private FileStream _fs = null;
	private ZipFile _zipFile = null;
#endif

	public void Init() {
#if !UNITY_EDITOR && UNITY_ANDROID
		_entries.Clear();

		_fs = File.OpenRead(Application.dataPath);
		_zipFile = new ZipFile(_fs);

		var e = _zipFile.GetEnumerator();
		while (e.MoveNext()) {
			ZipEntry zipEntry = e.Current as ZipEntry;

			if (zipEntry.Name.StartsWith("assets/")) {
				_entries.Add(zipEntry.Name, new Entry(zipEntry.ZipFileIndex, zipEntry.Size));
			}
		}
#endif
	}

	public void Close() {
#if !UNITY_EDITOR && UNITY_ANDROID
		if (_zipFile != null) {
			_zipFile.Close();
		}

		if (_fs != null) {
			_fs.Close();
		}
#endif
	}

	public byte[] Load(string filePath) {
		byte[] bytes = null;

#if !UNITY_EDITOR && UNITY_ANDROID
		string path = "assets/" + filePath;

		Entry entry;
		if (!_entries.TryGetValue(path, out entry)) {
			return null;
		}

		using (Stream s = _zipFile.GetInputStream(entry.index)) {
			bytes = new byte[entry.size];
			s.Read(bytes, 0, (int)entry.size);					
		}
#else
		string path = Path.Combine(Application.streamingAssetsPath, filePath);

		if (!File.Exists(path)) {
			return null;
		}

		bytes = File.ReadAllBytes(path);
#endif

		return bytes;
	}
}
