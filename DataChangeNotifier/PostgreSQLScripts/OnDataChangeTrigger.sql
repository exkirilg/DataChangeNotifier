CREATE TRIGGER "OnDataChange"
  AFTER INSERT OR DELETE OR UPDATE 
  ON public.<table_name>
  FOR EACH ROW
  EXECUTE PROCEDURE public."NotifyOnDataChange"()