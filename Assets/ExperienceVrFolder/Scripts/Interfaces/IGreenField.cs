public interface IGreenField {

    int chooseNum { get; set; }
    bool isCursor { get; set; }
    void SetDefault();
    void SetCursor();
    void SetObject(int idx);
    void DestroyObject();
}
