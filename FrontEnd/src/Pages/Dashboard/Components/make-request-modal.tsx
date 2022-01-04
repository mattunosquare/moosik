import { TextField } from "@mui/material";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import { Button } from "@mui/material";
import { Grid } from "@mui/material";

const style = {
  position: "absolute" as "absolute",
  top: "50%",
  left: "50%",
  transform: "translate(-50%, -50%)",
  width: 400,
  bgcolor: "background.paper",
  border: "2px solid #000",
  boxShadow: 24,
  p: 4,
};

const MakeRequestModal = () => {
  return (
    <div>
      <Box sx={style}>
        <Typography id="modal-header" variant="h6" component="h2">
          Make A Request
        </Typography>

        <Typography id="modal-subtitle" sx={{ mt: 2 }}>
          What're you up to? What do you need a recommendation for?
        </Typography>

        <TextField id="title-field" label="Title" variant="filled" fullWidth />

        <Typography id="further-details-subtitle" sx={{ mt: 2 }}>
          Any further details to provide?
        </Typography>

        <TextField
          id="further-details-field"
          label="Further Details"
          fullWidth
          multiline
          variant="filled"
          rows={4}
        />

        <Typography id="post-resources-subtitle" sx={{ mt: 2 }}>
          Want a similar song or to provide a jumping-off points? Please provide
          a link below.
        </Typography>

        <TextField
          id="post-resources-field"
          label="Please provide a link."
          fullWidth
          multiline
          variant="filled"
          rows={4}
        />
        <Grid container justifyContent="flex-end" gap="3%" sx={{ mt: "7%" }}>
          <Button size="small">Cancel</Button>
          <Button size="small">Save</Button>
        </Grid>
      </Box>
    </div>
  );
};

export default MakeRequestModal;
