
-- CREATE EXTENSION plpythonu;
 
CREATE or replace FUNCTION get_url_as_json (p_url text)
  RETURNS text
AS $$  
    import urllib, json
    from time import sleep
    
    try:
    
        sleep(0.75)
    
        response = urllib.urlopen(p_url)
    
        string_data = response.read()
        return string_data;
    except Exception as e:
    	return str(e)
    	
$$ LANGUAGE plpythonu;

 



 
