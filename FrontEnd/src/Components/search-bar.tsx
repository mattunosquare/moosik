import Autocomplete from "@mui/material/Autocomplete";
import TextField from "@mui/material/TextField";
import { useState } from "react";

const SearchAutoComplete = () => {
  interface IActivity {
    id: number;
    name: string;
  }

  const activities: IActivity[] = [
    { id: 1, name: "Gym" },
    { id: 2, name: "Programming" },
    { id: 3, name: "House Work" },
    { id: 4, name: "Running" },
    { id: 5, name: "Painting" },
  ];

  const [selectedActivity, setSelectedActivity] = useState<IActivity | null>();

  return (
    <Autocomplete
      id="Activity Search"
      options={activities}
      renderInput={(params) => (
        <TextField {...params} label="Search" variant="outlined" />
      )}
      getOptionLabel={(option) => option.name}
      value={selectedActivity}
    />
  );
};

export default SearchAutoComplete;
